using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using UnityEditor.PackageManager;
using System.Text;
using System.Threading;

public class ClientTCP : MonoBehaviour
{
    private static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private static byte[] buffer = new byte[512];
    private static byte[] sendBuffer = new byte[512];
    // Start is called before the first frame update


    public static void startClient(string IP)
    {
        // need a server IP variable that is modifed by the input field and it goes here instead of "127.0.0.1"
        //                      (IPAddress = userIPInput), 8888)
        client.Connect(IPAddress.Parse(IP), 8888);
        Debug.Log("Connected to the server!");

        //client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client);

        // the thread will execute tFunc()
        Thread t = new Thread(new ThreadStart(dumbassfunction));
        t.Name = "Recieve Thread";
        Console.WriteLine(t.Name + " has been created!");
        t.Start();

    }

    // Update is called once per frame
    void Update()
    {
        Send();
    }

     private static void dumbassfunction()
     {
        client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client);
     }


    private static void ReceiveCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int rec = socket.EndReceive(result);

        byte[] data = new byte[rec];
        Array.Copy(buffer, data, rec);

        string msg = Encoding.ASCII.GetString(data);
        Debug.Log("Received: " + msg);

        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);
    }

    private static void Send()
    {
        //int c = 0;
        //while (true)
        //{
        //c++;
        string sMsg = "Its Mohit's Birthday Party Tomorrow";
        sendBuffer = Encoding.ASCII.GetBytes(sMsg);

        client.Send(sendBuffer);

            //pretend interval
        Thread.Sleep(100);
        //}
    }

}
