using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame;
using MyGame.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyGame.World
{
    class Map
    {
        Texture2D[] textures;
        Tile[] tiles;
        Object[] objects;
        Texture2D collision;

        int collX, collY;

        internal Object[] Objects
        {
            get { return objects; }
            set { objects = value; }
        }

        const int TILE_WIDTH = 256;
        const int HALF_TILE_WIDTH = TILE_WIDTH / 2;
        const int TILE_HEIGHT = 128;
        const int HALF_TILE_HEIGHT = TILE_HEIGHT / 2;

        public Map()
        {
            ProcessMap("Data/Map.xml");
            ProcessObjects("Data/Objects.xml");

            textures = new Texture2D[objects.Count()];
        }

        public void ChangeTile(int x, int y)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].X == x && tiles[i].Y == y)
                {
                    for (int j = 0; j < tiles[i].ID.Length; j++)
                    {
                        tiles[i].ID[j] = 4;
                    }
                    break;
                }
            }
        }

        public void LoadContent()
        {
            for (int j = 0; j < objects.Count(); j++)
            {
                textures[j] = Globals.Content.Load<Texture2D>("Tiles/" + objects[j].ID);
            }
        }

        public void Update(GameTime gameTime, Player player)
        {
            Vector2 previousPosition = player.Position;

            // Check for collisions with blocking tiles
            foreach (var tile in tiles)
            {
                for (int j = 0; j < tile.ID.Length; j++)
                {
                    // Get the content ID of the tile
                    int contentId = tile.ID[j];

                    // Check if the tile is flagged as blocking
                    if (objects[contentId].IsUnpassable)
                    {
                        // Calculate isometric coordinates for the tile
                        int isoX = (tile.X - tile.Y) * HALF_TILE_WIDTH;
                        int isoY = (tile.X + tile.Y) * HALF_TILE_HEIGHT;

                        // Check for collision with the player
                        if (player.Position.X >= isoX && player.Position.X < isoX + TILE_WIDTH &&
                            player.Position.Y >= isoY && player.Position.Y < isoY + TILE_HEIGHT)
                        {
                            Console.WriteLine("Collision detected with a blocking tile.");
                            // Reset the player's position to its previous position
                            player.Position = previousPosition;
                            return; // Stop further collision checks
                        }
                    }
                }
            }
        }

        public void Draw()
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                // Calculate isometric coordinates
                int isoX = (tiles[i].X - tiles[i].Y) * HALF_TILE_WIDTH;
                int isoY = (tiles[i].X + tiles[i].Y) * HALF_TILE_HEIGHT;

                // Draw each content in the cell
                for (int j = 0; j < tiles[i].ID.Length; j++)
                {
                    // Assuming textures array contains textures for each content ID
                    int contentId = tiles[i].ID[j];
                    Globals.SpriteBatch.Draw(textures[contentId], new Vector2(isoX, isoY), Color.White);
                }
            }
        }

        public void ProcessMap(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNodeList tile = doc.GetElementsByTagName("Tile");

                tiles = new Tile[tile.Count];

                for (int i = 0; i < tile.Count; i++)
                {
                    tiles[i] = new Tile(Int32.Parse(tile[i].Attributes["x"].Value), Int32.Parse(tile[i].Attributes["y"].Value));

                    string contentValue = tile[i].InnerText.Trim();
                    string contentSubstring = contentValue.Substring(contentValue.IndexOf("{") + 1, contentValue.IndexOf("}") - contentValue.IndexOf("{") - 1);
                    string[] contentIds = contentSubstring.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    List<int> parsedIds = new List<int>();
                    foreach (string id in contentIds)
                    {
                        int parsedId;
                        if (int.TryParse(id, out parsedId))
                        {
                            parsedIds.Add(parsedId);
                        }
                    }

                    tiles[i].ID = parsedIds.ToArray();
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Data);
            }
        }

        public void ProcessObjects(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNodeList obj = doc.GetElementsByTagName("Object");
                objects = new Object[obj.Count];

                for (int i = 0; i < obj.Count; i++)
                {
                    objects[i] = new Object();
                    objects[i].ID = Int32.Parse(obj[i].Attributes["id"].Value);

                    if (obj[i].Attributes["flags"] != null)
                    {
                        objects[i].IsUnpassable = obj[i].Attributes["flags"].Value.Contains("Block");
                    }
                    else
                    {
                        objects[i].IsUnpassable = false;
                    }
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Data);
            }
        }

        public Vector2 IsoToCart(float isoX, float isoY)
        {
            float cartX = (2 * isoY + isoX) / 2;
            float cartY = (2 * isoY - isoX) / 2;
            return new Vector2(cartX, cartY);
        }

        public Vector2 CartToIso(float cartX, float cartY)
        {
            float isoX = cartX - cartY;
            float isoY = (cartX + cartY) / 2;
            return new Vector2(isoX, isoY);
        }
    }
}