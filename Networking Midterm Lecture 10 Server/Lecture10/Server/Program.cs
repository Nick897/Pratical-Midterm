﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO.Ports;

namespace Server
{
    internal class Program
    {

        private static byte[] buffer = new byte[512];
        private static byte[] sendBuffer = new byte[512];
        private static Socket server;
        private static string sendMsg = "";
        // Client list 
        private static List<Socket> clientSockets = new List<Socket>();

        static void Main(string[] args)
        {
            Console.WriteLine("Starting Server...");
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));

            server.Listen(10);

            server.BeginAccept(new AsyncCallback(AcceptCallback), null);

            //will create then start immediately
            Thread sendThread = new Thread(new ThreadStart(SendLoop));
            sendThread.Name = "SendThread";
            sendThread.Start();

            Console.ReadLine();
        }

        private static void AcceptCallback(IAsyncResult result)
        {
            Socket socket = server.EndAccept(result);
            Console.WriteLine("Client connected!");
            clientSockets.Add(socket);

            socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);

            server.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }

        private static void ReceiveCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            int rec = socket.EndReceive(result);
            byte[] data = new byte[rec];
            Array.Copy(buffer, data, rec);

            String msg = Encoding.ASCII.GetString(data);

            //// Here is where you protect the resource (buffer)
            sendMsg += " " + msg;

            socket.BeginReceive(buffer, 0 , buffer.Length, 0 , new AsyncCallback(ReceiveCallback), socket);
        }

        private static void SendCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndSend(result);
        }

        private static void SendLoop()
        {
            while(true)
            {
                sendBuffer = Encoding.ASCII.GetBytes(sendMsg);

                foreach (var socket in clientSockets)
                {
                    Console.WriteLine("Sent to: " + socket.RemoteEndPoint.ToString());

                    socket.BeginSend(sendBuffer,0,sendBuffer.Length, 0, new AsyncCallback(SendCallback), socket);
                }
                sendMsg = "";
                Thread.Sleep(1000);
            }
        }
    }
}
