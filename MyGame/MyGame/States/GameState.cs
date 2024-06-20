using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Entities;
using MyGame.Input;
using MyGame.UI;
using MyGame.UI.Chat;
using MyGame.World;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace MyGame.States
{
    public class GameState : IState
    {
        Camera camera;
        OrthogonalMap map;

        // NETWORKING STUFF
        public static NetworkStream stream;

        // logout panel
        Texture2D logoutPanel;
        Vector2 logoutPanelPosition;
        Button btnSubmit;
        Button btnCancel;
        
        bool isLogoutPressed;

        // icons with images
        Texture2D icons;

        Icon iconMenu;
        Icon backpackIcon;
        
        Icon skillsIcon;

        Texture2D textureBar;

        Icon gearIcon;
        Icon logoutIcon;

        InventoryTab inventory;

        Vector2 mouseInWorldToTilePos;

        // CHAT STUFF
        ChatManager chatManager;

        // player preview panel
        Texture2D playerPreview;

        // equipment slots
        Slot equipmentSlot1;
        Slot equipmentSlot2;
        Slot equipmentSlot3;
        Slot equipmentSlot4;

        // Panel
        Texture2D panel;
        Texture2D lineBReak;

        // skill tab
        SkillTab skillTab;

        // player stuff
        Player player;
        List<GameObject> gameObjects;

        // window close stuff from the internet
        // Windows API function declarations
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const int GWL_WNDPROC = -4;
        private const int WM_CLOSE = 0x0010;

        private delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

        private IntPtr originalWndProc;
        private WndProcDelegate newWndProcDelegate;

        public void Initialize()
        {
            // SEVER STUFF
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;
           
            Console.WriteLine("GameState Initialized!");

            Game1.Instance.Window.ClientSizeChanged += OnWindowResize;

            // Initialize camera and set its initial position to the player position
            camera = new Camera();
            camera.Zoom = 2f; // Ensure the camera zoom is set properly
            camera.Rotation = 0.0f; // Ensure the camera rotation is set properly
            camera.Position = new Vector2(570, 610);

            map = new OrthogonalMap();
            map.LoadContent();

            // Player
            player = new Player();

            // CHAT MANAGER
            chatManager = new ChatManager();

            // Player2
            gameObjects = new List<GameObject>();
            gameObjects.Add(new GameObject(1, 1));
            gameObjects.Add(new GameObject(2, 2));

            // menu icon
            iconMenu = new Icon(Globals.screenWidth - 24, 0);
            iconMenu.OnClick += MenuPressed;

            backpackIcon = new Icon(Globals.screenWidth - 196 + 4, Globals.screenHeight / 2 - 28);
            backpackIcon.OnClick += OpenBackpack;

            skillsIcon = new Icon(Globals.screenWidth - 196 + 4 + 24 + 4, Globals.screenHeight / 2 - 28);
            skillsIcon.OnClick += OpenSkill;

            gearIcon = new Icon(Globals.screenWidth - 24 - 4, Globals.screenHeight / 2 - 28);

            logoutIcon = new Icon(Globals.screenWidth - 24 - 4 - 24 - 4, Globals.screenHeight / 2 - 28);
            logoutIcon.OnClick += ShowPanel;

            Game1.Instance.Exiting += new EventHandler<EventArgs>(OnWindowClose);

            panel = Globals.Content.Load<Texture2D>("UI/panel");
            
            // player preview
            playerPreview = Globals.Content.Load<Texture2D>("Character/3398");

            // icons inside panel
            lineBReak = Globals.Content.Load<Texture2D>("UI/lineBreak");

            // slots
            inventory = new InventoryTab(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 + 8));

            // equipment slots
            equipmentSlot1 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot2 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 32 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot3 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 64 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot4 = new Slot(new Vector2(8 + Globals.screenWidth - 196 + 4 + 96 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));

            // icon images
            icons = Globals.Content.Load<Texture2D>("UI/icons");

            // skill tab
            textureBar = Globals.Content.Load<Texture2D>("UI/healthBar");
            skillTab = new SkillTab(new Vector2(0, 0));

            logoutPanel = Globals.Content.Load<Texture2D>("UI/logout_panel");
            logoutPanelPosition = new Vector2(Globals.screenWidth / 2 - 450 / 2, Globals.screenHeight / 2 - 134 / 2);

            btnSubmit = new Button((int)logoutPanelPosition.X + 64 + 32, (int)logoutPanelPosition.Y + 128 - 32, "Submit");
            btnSubmit.OnClick += SubmitPanel;

            btnCancel = new Button((int)logoutPanelPosition.X + 64 + 128 + 32 + 8, (int)logoutPanelPosition.Y + 128 - 32, "Cancel");
            btnCancel.OnClick += CancelLogout;

            isLogoutPressed = false;

            // NETWORK STUFF
            try
            {
                // Create a TCP client and connect to the server
                TcpClient client = new TcpClient();
                client.Connect(ipAddress, port);
                stream = client.GetStream();
                Console.WriteLine("Connected to server at {0}:{1}", ipAddress, port);

                // Send data to the server
                string text = "HI";
                string text2 = "HELLOO";

                SendData(text, stream);
                SendData(text2, stream);

                // Receive a response from the server
                string str1 = ReceiveData(stream);
                string str2 = ReceiveData(stream);

                // Close the client connection
                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            // closing stuff from the internet
            IntPtr hWnd = Game1.Instance.Window.Handle;
            newWndProcDelegate = new WndProcDelegate(WndProc);
            IntPtr newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newWndProcDelegate);
            originalWndProc = GetWindowLong(hWnd, GWL_WNDPROC);

            // Set custom window procedure
            SetWindowLong(hWnd, GWL_WNDPROC, newWndProcPtr);
        }

        // Custom window procedure to intercept messages
        private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
        {
            try
            {
                if (msg == WM_CLOSE)
                {
                    // Show confirmation panel instead of closing
                    isLogoutPressed = true;
                    Console.WriteLine("Intercepting WM_CLOSE, showing logout panel.");
                    return IntPtr.Zero; // Ignore the close message
                }

                // Validate originalWndProc before calling
                if (originalWndProc == IntPtr.Zero)
                {
                    Console.WriteLine("originalWndProc is null, cannot proceed with CallWindowProc.");
                    return IntPtr.Zero;
                }

                // Call original window procedure for other messages
                return CallWindowProc(originalWndProc, hWnd, msg, wParam, lParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in WndProc: " + ex.Message);
                return IntPtr.Zero;
            }
        }


        // Properly call the original window procedure
        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private void OnWindowClose(object sender, EventArgs e)
        {
            isLogoutPressed = true;
        }

        private void ShowPanel()
        {
            isLogoutPressed = true;
        }

        private void CancelLogout()
        {
            isLogoutPressed = false;
        }

        private void SubmitPanel()
        {
            // Restore the original window procedure to allow close messages
            SetWindowLong(Game1.Instance.Window.Handle, GWL_WNDPROC, originalWndProc);

            if (Game1.Instance != null)
            {
                Game1.Instance.Exit();
            }
            else
            {
                Console.WriteLine("Game1.Instance is null!");
            }
        }

        private void OpenSkill()
        {
            skillTab.IsVisible = !skillTab.IsVisible;
            if (skillTab.IsVisible)
            {
                inventory.IsVisible = false;
            }
        }

        private void OpenBackpack()
        {
            inventory.IsVisible = !inventory.IsVisible;
            if (inventory.IsVisible)
            {
                skillTab.IsVisible = false;
            }
        }

        private void OnWindowResize(object sender, EventArgs e)
        {
            inventory.UpdatePosition(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 + 8));
            equipmentSlot1.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot2.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 32 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot3.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 64 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));
            equipmentSlot4.UpdatePosition(new Vector2(8 + Globals.screenWidth - 196 + 4 + 96 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4 + 4, Globals.screenHeight / 2 - 28 - 24 - 18 - 16));

            skillTab.Position = new Vector2(inventory.Position.X, inventory.Position.Y);
        }

        private void MenuPressed()
        {
            iconMenu.isPressed = !iconMenu.isPressed;
        }

        private void GoBack()
        {
            Console.WriteLine("Back Button pressed.");
            Game1.stateManager.PopState(this);
            Game1.stateManager.PushState(new MenuState());
        }

        public void UnloadContent()
        {
            iconMenu.OnClick -= MenuPressed;
            Game1.Instance.Window.ClientSizeChanged -= OnWindowResize;
            Console.WriteLine("Disposing GameState.");
        }

        public void Update(GameTime gameTime)
        {
            // update map
            map.Update(gameTime);
            if (iconMenu.isPressed)
                iconMenu.Position = new Vector2(Globals.screenWidth - 196 - 24, 0);
            else
                iconMenu.Position = new Vector2(Globals.screenWidth - 24, 0);

            player.Update(gameTime);
            camera.Position = player.Position;

            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update(gameTime);
            }

            skillTab.Position = new Vector2(inventory.Position.X, inventory.Position.Y);

            // Mouse to World position;
            Matrix inverseMatrix = Matrix.Invert(camera.GetTransformation(Globals.Graphics));
            Vector2 mouseInWorld = Vector2.Transform(new Vector2(MouseHandler.GetMousePosition().X, MouseHandler.GetMousePosition().Y), inverseMatrix);
            mouseInWorldToTilePos = new Vector2(mouseInWorld.X / 32, mouseInWorld.Y / 32);

            if (MouseHandler.IsMouseRightPressed())
            {
                if ((int)(mouseInWorldToTilePos.X) == 0 && (int)(mouseInWorldToTilePos.Y) == 0)
                {
                    map.ChangeTile(0, 0);
                }
            }
            
            iconMenu.Update(gameTime);
            
            backpackIcon.Update(gameTime);
            backpackIcon.Position = new Vector2(Globals.screenWidth - 196 + 4, Globals.screenHeight / 2 - 28);
            skillsIcon.Update(gameTime);
            skillsIcon.Position = new Vector2(Globals.screenWidth - 196 + 4 + 24 + 4, Globals.screenHeight / 2 - 28);
            gearIcon.Update(gameTime);
            gearIcon.Position = new Vector2(Globals.screenWidth - 24 - 4, Globals.screenHeight / 2 - 28);
            logoutIcon.Update(gameTime);
            logoutIcon.Position = new Vector2(Globals.screenWidth - 24 - 4 - 24 - 4, Globals.screenHeight / 2 - 28);
            
            inventory.Update(gameTime);
            inventory.UpdatePosition(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 + 8));

            // skill tab
            skillTab.UpdatePosition(new Vector2(Globals.screenWidth - 196 + 8, Globals.screenHeight / 2 + 8 - 34));

            // logout panel
            logoutPanelPosition = new Vector2(Globals.screenWidth / 2 - 450 / 2, Globals.screenHeight / 2 - 134 / 2);

            // equipment slots
            equipmentSlot1.Update(gameTime);
            equipmentSlot2.Update(gameTime);
            equipmentSlot3.Update(gameTime);
            equipmentSlot4.Update(gameTime);

            // update logout menu panel
            if (isLogoutPressed)
            {
                btnSubmit.Update(gameTime);
                btnSubmit.btnPosition = new Vector2(logoutPanelPosition.X + 64 + 32, logoutPanelPosition.Y + 128 - 32);
                btnCancel.Update(gameTime);
                btnCancel.btnPosition = new Vector2(logoutPanelPosition.X + 64 + 128 + 32 + 8, logoutPanelPosition.Y + 128 - 32);
            }

            // CHAT STUFF
            chatManager.Update(gameTime);
        }

        public void Draw()
        {
            // Apply camera transformation when drawing the map and player
            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, camera.GetTransformation(Globals.Graphics));
            map.Draw();
            player.Draw();
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw();
            }
            Globals.SpriteBatch.End();

            // Draw UI elements without camera transformation
            Globals.SpriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null);
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, "Pos X: " + ((int)player.Position.X / (int)32) + ", Y: " + ((int)player.Position.Y / (int)32), new Vector2(0, 0), Color.White);
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, "MPos X: " + ((int)mouseInWorldToTilePos.X + ", Y: " + (int)mouseInWorldToTilePos.Y), new Vector2(0, 32), Color.White);

            player.DrawHealth();

            if (iconMenu.isPressed)
            {
                Globals.SpriteBatch.Draw(panel, new Rectangle(Globals.screenWidth - 196, 0, 196, Globals.screenHeight), Color.White);
                Globals.SpriteBatch.Draw(lineBReak, new Vector2(Globals.screenWidth - 196, Globals.screenHeight / 2), Color.White);
                Globals.SpriteBatch.Draw(lineBReak, new Vector2(Globals.screenWidth - 196, Globals.screenHeight / 2 - 32), Color.White);

                // Slots here
                inventory.Draw();

                // icons
                backpackIcon.Draw();
                skillsIcon.Draw();
                gearIcon.Draw();
                logoutIcon.Draw();

                // equipment slot
                equipmentSlot1.Draw();
                equipmentSlot2.Draw();
                equipmentSlot3.Draw();
                equipmentSlot4.Draw();

                Globals.SpriteBatch.Draw(playerPreview, new Rectangle(8 + Globals.screenWidth - 196 + 32, Globals.screenHeight / 2 - 32 - 180, 100, 100), Color.White);

                // icon images
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)backpackIcon.Position.X + 4, (int)backpackIcon.Position.Y + 4, 16, 16), new Rectangle(116, 10, 16, 16), Color.White);
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)skillsIcon.Position.X + 4, (int)skillsIcon.Position.Y + 4, 16, 16), new Rectangle(134, 10, 16, 16), Color.White);
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)gearIcon.Position.X + 4, (int)gearIcon.Position.Y + 4, 16, 16), new Rectangle(98, 10, 16, 16), Color.White);
                Globals.SpriteBatch.Draw(icons, new Rectangle((int)logoutIcon.Position.X + 4, (int)logoutIcon.Position.Y + 4, 16, 16), new Rectangle(80, 10, 16, 16), Color.White);

                skillTab.Draw();
            }

            if (isLogoutPressed)
            {
                Globals.SpriteBatch.Draw(logoutPanel, logoutPanelPosition, Color.White);
                Globals.SpriteBatch.DrawString(Globals.SpriteFont, "Are you sure you want to exit?", new Vector2(logoutPanelPosition.X+8, logoutPanelPosition.Y+8), Color.White);

                btnSubmit.Draw();
                btnCancel.Draw();
            }
            
            iconMenu.Draw();
            chatManager.Draw();
            Globals.SpriteBatch.End();
        }

        public static string ReceiveData(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = stream.Read(buffer, 0, buffer.Length);
            string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            Console.WriteLine("Received: {0}", response);
            return response;
        }

        public static void SendData(string message, NetworkStream stream)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
            Console.WriteLine("Sent: {0}", message);
        }
    }
}
