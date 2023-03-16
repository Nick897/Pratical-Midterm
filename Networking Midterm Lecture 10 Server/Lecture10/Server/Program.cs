using System;
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
        private static Socket serverUDP;
        private static string sendMsg = "";
        public static float[] pos;

        // Client list 
        private static List<Socket> clientSockets = new List<Socket>();

        static void Main(string[] args)
        {
            // TCP Socket
            Console.WriteLine("Starting Server...");
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            server.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8888));
            server.Listen(10);
            server.BeginAccept(new AsyncCallback(AcceptCallback), null);

            //UDP Socket
            serverUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            serverUDP.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8889));
            //clientSockets.Add(serverUDP);

            //Thread for starting Aysnc UDP Receive Callback
            Thread udpReceiveThread = new Thread(new ThreadStart(dumbassfunction2));
            udpReceiveThread.Name = "UDPReceiveThread";
            udpReceiveThread.Start();

            //Thread that Executes sending TCP messages to clients
            Thread sendThread = new Thread(new ThreadStart(SendLoop));
            sendThread.Name = "SendThread";
            sendThread.Start();

            Console.ReadLine();
        }
        //function that literally just calls our UDP receive. Its called by a Thread: udpReceiveThread
        private static void dumbassfunction2()
        {
            serverUDP.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), serverUDP);
        }
        //TCP Accept Callback Async Function
        private static void AcceptCallback(IAsyncResult result)
        {
            Socket socket = server.EndAccept(result);
            Console.WriteLine("Client connected!");
            clientSockets.Add(socket);

            socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);

            server.BeginAccept(new AsyncCallback(AcceptCallback), null);
        }
        //TCP ReceiveCallback Async Function
        private static void ReceiveCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            int rec = socket.EndReceive(result);
            byte[] data = new byte[rec];
            Array.Copy(buffer, data, rec);

            String msg = Encoding.ASCII.GetString(data);

            //// Here is where you protect the resource (buffer)
            sendMsg += " " + msg;

            socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);
        }

        //UDP ReceiveCallback Async Function
        private static void ReceiveUDPCallback(IAsyncResult result)
        {
            //recieve a message
            Socket socket = (Socket)result.AsyncState;
            int rec = socket.EndReceive(result);

            pos = new float[rec / 4];
            Buffer.BlockCopy(buffer, 0, pos, 0, rec);

            Console.WriteLine("Recieved From Client 1 X:" + pos[0] + " Y:" + pos[1] + " Z:" + pos[2]);

            socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), socket);
        }
        //TCP SendCallback Async Function
        private static void SendCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndSend(result);
        }
        // a function that sends the tcp messages received by all clients and outputs it to all other clients in the socket list
        // this function is called by a thread: sendThread
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
                Thread.Sleep(100);
            }
        }
    }
}
