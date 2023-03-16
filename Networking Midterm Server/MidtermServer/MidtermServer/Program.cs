using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net;
using System.Net.Sockets;


namespace MidtermServer
{
    internal class Program
    {
       public static byte[] buffer = new byte[512];
       public static float[] pos;
        public static void StartServer()
        {

        IPHostEntry hostInfo = Dns.GetHostEntry(Dns.GetHostName());

            //IPAddress ip = hostInfo.AddressList[1];
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            Console.WriteLine("Server name:{0} IP: {1}", hostInfo.HostName, ip);

            IPEndPoint localEP = new IPEndPoint(ip, 8889);

            Socket server = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);
            server.Blocking = false;
            server.Bind(localEP);

            //server.BeginReceive(new AsyncCallback(ReceiveCallback), null);
            // 0 is for any avaliable  port
            //EndPoint remoteClient = new IPEndPoint(IPAddress.Any, 8889);

            try
            {
                Console.WriteLine("Waiting for data...");
                // this one works rn
                server.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), server);
                //while (true)
                //{
                    //int recv = server.ReceiveFrom(buffer, ref remoteClient);
                    //int recv = server.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), server)
                    //server.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), server);
                //}

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

        }

        private static void RecieveCallback(IAsyncResult result)
        {
            //recieve a message
            Socket socket = (Socket)result.AsyncState;
            int rec = socket.EndReceive(result);

            pos = new float[rec / 4];
            Buffer.BlockCopy(buffer, 0, pos, 0, rec);

            //socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), socket);
            // if the identifyer at pos position 4 is = then its client 1 and if its 1 then its client 2
            if(pos[3] == 0)
            {
                Console.WriteLine("Recieved From Client 1 X:" + pos[0] + " Y:" + pos[1] + " Z:" + pos[2]);
            }
            socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), socket);
            socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), socket);
        }

        private static void SendCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            socket.EndSend(result);
        }

        public static int Main(String[] args)
        {
            StartServer();
            Console.ReadKey();
            return 0;
        }

        /* private static byte[] buffer = new byte[512];
         private static float[] pos;
         private static Socket server;

         static void Main(string[] args)
         {
             Console.WriteLine("Starting the Server...");

             server = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

             server.Blocking = false;
             //server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
             IPAddress ip = IPAddress.Parse("127.0.0.1");

             //server.Bind(new IPEndPoint(ip, 8889));
             IPEndPoint localEP = new IPEndPoint(ip, 8889);
             EndPoint remoteClient = new IPEndPoint(IPAddress.Any, 0);

             server.Bind(localEP);
             // server.Listen(10);
             server.Blocking = false;


             server.BeginAccept(new AsyncCallback(AcceptCallback), null);

             Console.Read();
         }

         private static void AcceptCallback(IAsyncResult result)
         {
             Socket client = server.EndAccept(result);
             Console.WriteLine("Client Connected!!! IP:");

             client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), client);
         }

         private static void RecieveCallback(IAsyncResult result)
         {
             //recieve a message
             Socket socket = (Socket)result.AsyncState;
             int rec = socket.EndReceive(result);

             //lecture 06
             // setting pos equal to the bytes we received divided by 4
             pos = new float[rec / 4];
             Buffer.BlockCopy(buffer, 0, pos, 0, rec);

             //lecture 06
             socket.BeginSend(buffer, 0, buffer.Length, 0, new AsyncCallback(SendCallback), socket);

             Console.WriteLine("Recieved From Client - X:" + pos[0] + " Y:" + pos[1] + " Z:" + pos[2]);

             socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), socket);
         }
         private static void SendCallback(IAsyncResult result)
         {
             Socket socket = (Socket)result.AsyncState;
             socket.EndSend(result);
         }
        */
    }

}
