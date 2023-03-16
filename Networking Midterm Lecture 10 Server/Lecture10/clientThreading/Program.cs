using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace clientThreading
{
    internal class Program
    {
        private static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static byte[] buffer = new byte[512];
        private static byte[] sendBuffer = new byte[512];

        static void Main(string[] args)
        {
            client.Connect(IPAddress.Parse("127.0.0.1"), 11111);
            Console.WriteLine("Connected to the server!");

            client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client);

            Send();

            Console.ReadLine();
        }

        private static void Send()
        {
            int c = 0;
            while(true)
            {
                c++;
                sendBuffer = Encoding.ASCII.GetBytes(c.ToString());

                client.Send(sendBuffer);
                
                //pretend interval
                Thread.Sleep(1000);
            }
        }

        private static void ReceiveCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;
            int rec = socket.EndReceive(result);

            byte[] data = new byte[rec];
            Array.Copy(buffer, data, rec);

            string msg = Encoding.ASCII.GetString(data);
            Console.WriteLine("Received: " + msg);

            socket.BeginReceive(buffer,0,buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);
        }
    }
}
