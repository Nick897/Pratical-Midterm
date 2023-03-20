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
        private static byte[] udpClient2Buffer = new byte[512];
        private static byte[] ReceiveFromBuffer = new byte[512];
        // client 1
        private static Socket serverUDP;
        private static EndPoint remoteClient1;
        public static byte[] bpos = new byte[512];
        public static float[] pos;
        // client 2
        private static Socket serverUDP2;
        private static EndPoint remoteClient2;
        public static byte[] bpos2 = new byte[512];
        public static float[] pos2;

        private static IPAddress udpIP;

        public static bool SomethingToSend = false;
        public static bool Client1ToSend = false;
        public static bool Client2ToSend = false;
        public static bool remoteClient1Changed = false;
        public static bool remoteClient2Changed = false;

        static void Main(string[] args)
        {

            IPAddress ip = IPAddress.Parse("127.0.0.1");
            IPEndPoint localEP = new IPEndPoint(ip, 8889);

            serverUDP = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            remoteClient1 = new IPEndPoint(IPAddress.Any, 0);
            remoteClient2 = new IPEndPoint(IPAddress.Any, 0);

            try
            {
                serverUDP.Bind(localEP);
                Console.WriteLine("Waiting for Data");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            serverUDP.BeginReceive(udpBuffer, 0, udpBuffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), serverUDP);

            Thread udpSendThread = new Thread(new ThreadStart(SendUDPPos));
            udpSendThread.Name = "UDPSendThread";
            udpSendThread.Start();


            Console.ReadLine();
        }

        //private static void dumbassfunction2()
        //{
        //    serverUDP.BeginReceive(udpBuffer, 0, udpBuffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), serverUDP);
        //}

        //UDP ReceiveCallback Async Function
        private static void ReceiveUDPCallback(IAsyncResult result)
        {
            //recieve a message
            Socket socket = (Socket)result.AsyncState;
            int rec = socket.EndReceive(result);
 

            pos = new float[rec / 4];
            Buffer.BlockCopy(udpBuffer, 0, pos, 0, rec);
            
            // if what we received is from client 1 then store the endpoint info of client 1 into remoteClient1
            if(pos[3] == 0)
            {
                Console.WriteLine("Recieved From Client 1 X:" + pos[0] + " Y:" + pos[1] + " Z:" + pos[2] + "ID = " + pos[3]);
                int recv = serverUDP.ReceiveFrom(ReceiveFromBuffer, ref remoteClient1);
                remoteClient1Changed = true;
                ReceiveFromBuffer = udpBuffer;
                Client2ToSend = true;
            }
            // if what we received is from client 2 then store the endpoint info of client 2 into remoteClient2
            if (pos[3] == 1)
            {
                Console.WriteLine("Recieved From Client 2 X:" + pos[0] + " Y:" + pos[1] + " Z:" + pos[2] + "ID = " + pos[3]);
                int recv = serverUDP.ReceiveFrom(udpClient2Buffer, ref remoteClient2);
                remoteClient2Changed = true;
                udpClient2Buffer = udpBuffer;
                Client1ToSend = true;
            }

            SomethingToSend = true;
            socket.BeginReceive(udpBuffer, 0, udpBuffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), socket);
        }

        // UDP Send Positions
        private static void SendUDPPos()
        {
            while(true)
            {
                if (SomethingToSend == true)
                {
                    //serverUDP.SendTo(udpBuffer, remoteClient1);
                    if(Client1ToSend == true)
                    {
                        if(remoteClient1Changed == true)
                        {
                            serverUDP.SendTo(ReceiveFromBuffer, remoteClient1);
                            Console.WriteLine("Sent UDP Positions to Client 1");
                            Client1ToSend = false;
                        }                      
                    }
                    if(Client2ToSend == true)
                    {
                        if (remoteClient2Changed == true)
                        {
                            serverUDP.SendTo(udpClient2Buffer, remoteClient2);
                            Console.WriteLine("Sent UDP Positions to Client 2");
                            Client2ToSend = false;
                        }
                    }

                    SomethingToSend = false;
                }
            }
        }
    }
}
