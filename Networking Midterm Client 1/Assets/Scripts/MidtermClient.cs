//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;
//using UnityEditor.PackageManager;
//using Unity.VisualScripting;
//using System.Threading;
//
//public class MidtermClient : MonoBehaviour
//{
//    private static Socket client = new Socket(AddressFamily.InterNetwork,
//    SocketType.Stream, ProtocolType.Tcp);
//    private static byte[] buffer = new byte[512];
//
//    public GameObject myCube;
//    private static byte[] buffer = new byte[512];
//    // the server end point
//    private static IPEndPoint remoteEP;
//    private static Socket clientSoc;
//
//    //Lecture 06
//    private static byte[] bpos;
//    private static float[] pos;
//
//    public void StartClient()
//    {
//        try
//        {
//            //IPAddress ip = IPAddress.Parse("127.0.0.1");
//            //remoteEP = new IPEndPoint(ip, 8889);
//
//            clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
//            clientSoc.Connect(IPAddress.Parse("127.0.0.1"), 8889);
//            Console.WriteLine("Connected to server!!!");
//
//            // this is Async
//            clientSoc.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), clientSoc);
//        }
//        catch (Exception e)
//        {
//            Debug.Log("Exception: " + e.ToString());
//        }
//    }
//
//    private void RecieveCallback(IAsyncResult results)
//    {
//        Socket socket = (Socket)results.AsyncState;
//        int rec = socket.EndReceive(results);
//
//        pos = new float[] { gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z };
//        Debug.Log("Client X:" + pos[0] + " Client Y:" + pos[1] + " Client Z:" + pos[2]);
//        bpos = new byte[pos.Length * 4];
//        // source with offset, destination with offset and length
//        Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);
//
//        clientSoc.Send(bpos);
//
//
//        // creates a loop by calling the function again
//        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(RecieveCallback), socket);
//    }
//
//       // Start is called before the first frame update
//    void Start()
//    {
//      // set the cube's starting position to its last position
//      //cubeLastPos = myCube.transform.position;
//      StartClient();
//    }
//
//    // Update is called once per frame
//    void Update()
//    {
//
//    }
//}
