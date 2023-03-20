using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEditor.PackageManager;
using System.Text;
using System.Threading;

public class ClientTCP : MonoBehaviour
{
    private static Socket client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    private static byte[] buffer = new byte[512];
    private static byte[] sendBuffer = new byte[512];
    private static string input = "";

    private string Chatinput;
    public GameObject inputField;
    public GameObject myText;
    private static string msg;
    // Start is called before the first frame update


    public static void startClient(string IP)
    {
        // need a server IP variable that is modifed by the input field and it goes here instead of "127.0.0.1"
        //                      (IPAddress = userIPInput), 8888)
        client.Connect(IPAddress.Parse(IP), 8888);
        Debug.Log("Connected to the server!");

        //client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client);

        // the thread will execute tFunc()
        Thread t = new Thread(new ThreadStart(PassOverFunction));
        t.Name = "Recieve Thread";
        Console.WriteLine(t.Name + " has been created!");
        t.Start();

    }

    // Update is called once per frame
    void Update()
    {
        Send();
        StoreInput();
    }

    private static void PassOverFunction()
    {
        client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client);
    }


    private static void ReceiveCallback(IAsyncResult result)
    {
        Socket socket = (Socket)result.AsyncState;
        int rec = socket.EndReceive(result);

        byte[] data = new byte[rec];
        Array.Copy(buffer, data, rec);

        msg = Encoding.ASCII.GetString(data);

        if(msg == null)
        {
            msg = "Client 2 Has Disconnected";
        }

        Debug.Log("Received: " + msg);
       // StoreInput();

        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), socket);
    }

    private static void Send()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            //int c = 0;
            //while (true)
            //{
            //c++;
            sendBuffer = Encoding.ASCII.GetBytes(input);

            client.Send(sendBuffer);

            //pretend interval
            Thread.Sleep(100);
            //}
        }
    }

    public static void ReadInput(string s)
    {
        input = s;
    }

    public void StoreInput()
    {
        //Chatinput = inputField.GetComponent<Text>().text;
        myText.GetComponent<Text>().text = "Received:" + msg;
    }

}
