using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MyGame.Input;
using MyGame.States;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MyGame.UI.Chat
{
    class ChatManager
    {
        private ChatBox chatBox;
        private TextBox textBox;

        private StringBuilder inputText;  // Store the typed text
        public StringBuilder InputText
        {
            get { return inputText; }
        }

        private bool isTyping = false;
        private bool showCursor = true;
        private float cursorTimer = 0f;  // Timer to control cursor blinking
        private float cursorInterval = 0.5f;  // Interval (in seconds) for cursor blinking

        private List<string> texts;

        public ChatManager()
        {
            chatBox = new ChatBox(0, Globals.screenHeight - 152);
            textBox = new TextBox(0, Globals.screenHeight - 10);
            textBox.IsClicked = false;

            texts = new List<string>();
            inputText = new StringBuilder();

            // Start listening for incoming messages from the server
            Task.Factory.StartNew(() => ListenForMessages(GameState.stream));
        }

        public void Update(GameTime gameTime)
        {
            if (KeyboardHandler.IsKeyJustPressed(Keys.Enter))
            {
                textBox.IsClicked = !textBox.IsClicked;

                if (textBox.IsClicked)
                {
                    // If textbox is clicked (activated), start typing
                    isTyping = true;
                }
                else
                {
                    // If textbox is not clicked (deactivated), send text and stop typing
                    if (inputText.Length > 0)
                    {
                        texts.Add(inputText.ToString());
                        try
                        {
                            SendData(inputText.ToString(), GameState.stream);
                        }
                        catch (ObjectDisposedException ex)
                        {
                            Console.WriteLine("Error: " + ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An unexpected error occurred: " + ex.Message);
                        }
                        inputText.Clear();
                    }
                    isTyping = false;
                }
            }

            if (textBox.IsClicked)
            {
                // Update cursor blinking
                cursorTimer += (float)gameTime.ElapsedGameTime.TotalSeconds;
                if (cursorTimer >= cursorInterval)
                {
                    cursorTimer = 0f;
                    showCursor = !showCursor; // Toggle cursor visibility
                }

                // Handle text input
                HandleTextInput();
            }

            textBox.Update(gameTime);
        }

        public void Draw()
        {
            chatBox.Draw();

            Vector2 drawPosition = chatBox.Position;
            foreach (string txt in texts)
            {
                Globals.SpriteBatch.DrawString(Globals.SpriteFont, txt, drawPosition, Color.White, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0);
                drawPosition.Y += Globals.SpriteFont.MeasureString(txt).Y * 0.5f - 18;
            }

            if (isTyping && showCursor)
            {
                Vector2 textSize = Globals.SpriteFont.MeasureString(inputText.ToString());
                Vector2 cursorPosition = new Vector2(textBox.Position.X - 2 + textSize.X, textBox.Position.Y - 6); // Adjust according to your chatbox position

                Globals.SpriteBatch.DrawString(Globals.SpriteFont, "|", cursorPosition, Color.White);
            }
            // Draw the typed text
            textBox.Draw();
            Globals.SpriteBatch.DrawString(Globals.SpriteFont, inputText.ToString(), new Vector2(textBox.Position.X - 2, textBox.Position.Y - 6), Color.White);
        }

        private void HandleTextInput()
        {
            Keys[] keys = KeyboardHandler.CurrentKeyboardState.GetPressedKeys();
            bool shiftPressed = KeyboardHandler.IsKeyPressed(Keys.LeftShift) || KeyboardHandler.IsKeyPressed(Keys.RightShift);

            foreach (Keys key in keys)
            {
                if (KeyboardHandler.IsKeyJustPressed(key))
                {
                    // Handle backspace
                    if (key == Keys.Back && inputText.Length > 0)
                    {
                        inputText.Remove(inputText.Length - 1, 1);
                    }
                    else
                    {
                        // Handle normal characters
                        char? character = ConvertKeyToChar(key, shiftPressed);
                        if (character != null)
                        {
                            inputText.Append(character);
                        }
                    }
                }
            }
        }

        private char? ConvertKeyToChar(Keys key, bool shiftPressed)
        {
            // Convert key to corresponding character, handling both uppercase and lowercase
            if (key >= Keys.A && key <= Keys.Z)
            {
                char letter = (char)(key - Keys.A + 'A');
                return shiftPressed ? letter : char.ToLower(letter);
            }
            if (key >= Keys.D0 && key <= Keys.D9)
            {
                return (char)(key - Keys.D0 + '0');
            }
            if (key == Keys.Space)
            {
                return ' ';
            }
            // Add more keys if needed

            return null;
        }

        public void AddText(string txt)
        {
            texts.Add(txt);
        }

        private static void SendData(string message, NetworkStream stream)
        {
            if (stream == null)
            {
                Console.WriteLine("Cannot send data, stream is null.");
                return;
            }

            if (!stream.CanWrite)
            {
                Console.WriteLine("Cannot send data, stream is not writable.");
                return;
            }

            try
            {
                byte[] data = Encoding.ASCII.GetBytes(message);
                stream.Write(data, 0, data.Length);
                Console.WriteLine("Sent: {0}", message);
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }

        private void ListenForMessages(NetworkStream stream)
        {
            if (stream == null)
            {
                Console.WriteLine("Cannot listen for messages, stream is null.");
                return;
            }

            if (!stream.CanRead)
            {
                Console.WriteLine("Cannot listen for messages, stream is not readable.");
                return;
            }

            try
            {
                while (true)
                {
                    byte[] buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0)
                    {
                        // Connection closed
                        break;
                    }

                    string message = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine("Received: {0}", message);

                    // Add received message to texts to display in chat
                    AddText(message);
                }
            }
            catch (ObjectDisposedException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: " + ex.Message);
            }
        }
    }
}
