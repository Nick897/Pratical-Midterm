using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using System.IO.Ports;

namespace ServerUDP
{
    internal class Program
    {
        private static byte[] udpBuffer = new byte[512];
        private static byte[] UDPOutBuffer = new byte[512];
        private static byte[] ReceiveFromBuffer = new byte[512];
        private static Socket serverUDP;
        //private static IPEndPoint remoteEP;
        private static EndPoint remoteClient;
        private static IPAddress udpIP;
        public static byte[] bpos = new byte[512];
        public static float[] pos;
        public static bool SomethingToSend = false;

        static void Main(string[] args)
        {
            //UDP Socket
            //serverUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            ////serverUDP.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8889));
            //udpIP = IPAddress.Parse("127.0.0.1");
            //remoteEP = new IPEndPoint(udpIP, 8889);
            //serverUDP.Bind(remoteEP);

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEP = new IPEndPoint(ip, 8889);

            serverUDP = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            remoteClient = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                serverUDP.Bind(localEP);
                Console.WriteLine("Waiting for Data");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }


            //Thread for starting Aysnc UDP Receive Callback
            //Thread udpReceiveThread = new Thread(new ThreadStart(dumbassfunction2));
            //udpReceiveThread.Name = "UDPReceiveThread";
            //udpReceiveThread.Start();

            serverUDP.BeginReceive(udpBuffer, 0, udpBuffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), serverUDP);

            Thread udpSendThread = new Thread(new ThreadStart(SendUDPPos));
            udpSendThread.Name = "UDPSendThread";
            udpSendThread.Start();

            //SendUDPPos();

            Console.ReadLine();
        }

        private static void dumbassfunction2()
        {
            serverUDP.BeginReceive(udpBuffer, 0, udpBuffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), serverUDP);
        }

        //UDP ReceiveCallback Async Function
        private static void ReceiveUDPCallback(IAsyncResult result)
        {
            //recieve a message
            Socket socket = (Socket)result.AsyncState;
            int rec = socket.EndReceive(result);

            int recv = serverUDP.ReceiveFrom(ReceiveFromBuffer, ref remoteClient);

            pos = new float[rec / 4];
            Buffer.BlockCopy(udpBuffer, 0, pos, 0, rec);

            SomethingToSend = true;
            Console.WriteLine("Recieved From Client 1 X:" + pos[0] + " Y:" + pos[1] + " Z:" + pos[2]);

            socket.BeginReceive(udpBuffer, 0, udpBuffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), socket);
        }

        // UDP Send Positions
        private static void SendUDPPos()
        {
            while(true)
            {
                if (SomethingToSend == true)
                {

                    //bpos = new byte[pos.Length * 4];
                    //Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

                    //serverUDP.SendTo(udpBuffer, remoteEP);
                    serverUDP.SendTo(udpBuffer, remoteClient);
                    Console.WriteLine("Sent UDP Positions to Client");

                    SomethingToSend = false;
               }
            }
        }
    }
}
