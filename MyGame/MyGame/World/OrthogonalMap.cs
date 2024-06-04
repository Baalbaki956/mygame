using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MyGame.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MyGame.World
{
    class OrthogonalMap
    {
        Texture2D[] textures;
        Tile[] tiles;

        internal Tile[] Tiles
        {
            get { return tiles; }
            set { tiles = value; }
        }
        Object[] objects;
        Action[] actions;

        internal Action[] Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        internal Object[] Objects
        {
            get { return objects; }
            set { objects = value; }
        }

        const int TILE_WIDTH = 32;
        const int TILE_HEIGHT = 32;

        public OrthogonalMap()
        {
            ProcessMap("Data/MapOrtho.xml");
            ProcessObjects("Data/ObjectsOrtho.xml");
            ProcessActions("Data/Actions.xml");

            textures = new Texture2D[objects.Count()];
        }

        public void LoadContent()
        {
            for (int j = 0; j < objects.Count(); j++)
            {
                textures[j] = Globals.Content.Load<Texture2D>("Tiles/" + objects[j].ID);
            }
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw()
        {
            foreach (var tile in tiles)
            {
                for (int j = 0; j < tile.ID.Length; j++)
                {
                    int contentId = tile.ID[j];
                    Vector2 position = new Vector2(tile.X * TILE_WIDTH, tile.Y * TILE_HEIGHT);
                    Globals.SpriteBatch.Draw(textures[contentId], position, Color.White);
                }
            }
        }

        public void ChangeTile(int x, int y)
        {
            for (int i = 0; i < tiles.Length; i++)
            {
                if (tiles[i].X == x && tiles[i].Y == y)
                {
                    for (int j = 0; j < tiles[i].ID.Length; j++)
                    {
                        tiles[i].ID[j] = 0;
                    }
                    break;
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

                    // Check if the "flags" attribute exists
                    if (obj[i].Attributes["flag"] != null && obj[i].Attributes["flag"].Value.Contains("Unpass"))
                    {
                        objects[i].IsUnpassable = true;
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public void ProcessActions(string path)
        {
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(path);

                XmlNodeList action = doc.GetElementsByTagName("Action");
                actions = new Action[action.Count];

                for (int i = 0; i < action.Count; i++)
                {
                    actions[i] = new Action(Int32.Parse(action[i].Attributes["actionID"].Value), action[i].Attributes["name"].Value);
                }
            }
            catch (XmlException e)
            {
                Console.WriteLine(e.Data);
            }
        }
    }
}
