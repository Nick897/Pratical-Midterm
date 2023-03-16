using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEditor.PackageManager;
using Unity.VisualScripting;
using System.Threading;

public class Chatbox : MonoBehaviour
{
    private string input;
    public GameObject inputField;
    public GameObject myText;

    //private static Socket client = new Socket(AddressFamily.InterNetwork,
    //SocketType.Stream, ProtocolType.Tcp);
    //private static byte[] buffer = new byte[512];

    //public static Text myText;

    // Start is called before the first frame update
    //void Start()
    //{
    //    client.Connect(IPAddress.Parse("127.0.0.1"), 8888);
    //    Debug.Log("Connected to server!!!");
    //    //client.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveCallback), client);
    //    //SyncSend();
    //    
    //}

    public void StoreInput()
    {
        input = inputField.GetComponent<Text>().text;
        myText.GetComponent<Text>().text = "Client 1:" + input;
    }

   //private static void ReceiveCallback(IAsyncResult results)
   //{
   //    Socket socket = (Socket)results.AsyncState;
   //    int rec = socket.EndReceive(results);
   //    byte[] data = new byte[rec];
   //    Array.Copy(buffer, data, rec);
   //    String msg = Encoding.ASCII.GetString(data);
   //    Debug.Log("Recv: " + msg);
   //    myText = myText.GetComponent<Text>();
   //    socket.BeginReceive(buffer, 0, buffer.Length, 0,
   //        new AsyncCallback(ReceiveCallback), socket);
   //}

  

    // Update is called once per frame
    //void Update()
    //{
       
    //}
}
