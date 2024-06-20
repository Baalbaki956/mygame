using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class Server
    {
        public static void Main(string[] args)
        {
            // Define the IP address and port for the server to listen on
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;

            // Create a TCP listener to listen for incoming connections
            TcpListener listener = new TcpListener(ipAddress, port);

            try
            {
                // Start the listener
                listener.Start();
                Console.WriteLine("Server started on {0}", port);

                // Accept an incoming connection
                Socket clientSocket = listener.AcceptSocket();
                Console.WriteLine("Client connected");

                // Receive data from the client
                //ReceiveData(clientSocket);
                //ReceiveData(clientSocket);

                // Send a response back to the client
                string message = ReceiveData(clientSocket);
                SendData(message, clientSocket);

                string message2 = ReceiveData(clientSocket);
                SendData(message2, clientSocket);

                // Close the client connection
                clientSocket.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                // Stop the listener
                listener.Stop();
            }

            Console.ReadLine();
        }

        public static string ReceiveData(Socket clientSocket)
        {
            byte[] buffer = new byte[1024];
            int bytesReceived = clientSocket.Receive(buffer);
            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesReceived);
            Console.WriteLine("Received: {0}", dataReceived);
            return dataReceived;
        }

        public static void SendData(string message, Socket clientSocket)
        {
            // Send a response back to the client
            byte[] msgData = Encoding.ASCII.GetBytes(message);
            clientSocket.Send(msgData);
            Console.WriteLine("Sent: {0}", message);
        }
    }
}
