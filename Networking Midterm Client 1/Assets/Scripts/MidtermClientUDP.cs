using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using UnityEditor.PackageManager;
using System.Threading;

public class MidtermClientUDP : MonoBehaviour
{
    //public GameObject myCube;
    private static byte[] buffer = new byte[512];
    // the server end point
    private static IPEndPoint remoteEP;
    private static Socket clientSoc;

    private static byte[] bpos;
    private static float[] pos;
    // Identifyer should be 0 in client 1 & 1 in Client 2
    private static float identifier = 0;

    private Vector3 cubeLastPos;
    public GameObject remoteCube;

    private static Queue<float[]> RemotePosQueue = new Queue<float[]>();
    private static float[] tempfloatArray;

    public static void StartClient()
    {
        try
        {
            //IPAddress ip = IPAddress.Parse("192.168.2.178");
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            remoteEP = new IPEndPoint(ip, 8889);

            clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            //clientSoc = new Socket(ip.AddressFamily, SocketType.Dgram, ProtocolType.Udp);

            // this is Async not needed rn
            //clientSoc.BeginReceive(bpos, 0, bpos.Length, 0, new AsyncCallback(ReceiveUDPCallback), clientSoc);

            // the thread will execute tFunc()
            Thread t = new Thread(new ThreadStart(PassOverFunction3UDP));
            t.Name = "Recieve UDP Thread";
            t.Start();
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }
    private static void PassOverFunction3UDP()
    {
        Debug.Log("Dumbass Function Started");
        clientSoc.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), clientSoc);
    }

    // Start is called before the first frame update
    void Start()
    {
        // set the cube's starting position to its last position
        cubeLastPos = gameObject.transform.position;
        StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        //only send cube position if it as moved from its last position
        if (gameObject.transform.position != cubeLastPos)
        {
            SendCubePos();
        }
        cubeLastPos = gameObject.transform.position;

        if(RemotePosQueue.Count > 0)
        {
            tempfloatArray = RemotePosQueue.Dequeue();
            //remoteCube.transform.position.x = tempfloatArray[0];
            remoteCube.transform.position = new Vector3(tempfloatArray[0], tempfloatArray[1], tempfloatArray[2]);
        }
    }
    public void SendCubePos()
    {
        pos = new float[] { gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z, identifier};
        bpos = new byte[pos.Length * 4];
        // source with offset, destination with offset and length
        Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

        clientSoc.SendTo(bpos, remoteEP);
    }
    private static void ReceiveUDPCallback(IAsyncResult result)
    {
        //recieve a message
        Socket socket = (Socket)result.AsyncState;
        int rec = socket.EndReceive(result);

        pos = new float[rec / 4];
        Buffer.BlockCopy(buffer, 0, pos, 0, rec);

        Debug.Log("Recieved From Server: X: " + pos[0] + " Y:" + pos[1] + " Z:" + pos[2]);
        RemotePosQueue.Enqueue(pos);

        socket.BeginReceive(buffer, 0, buffer.Length, 0, new AsyncCallback(ReceiveUDPCallback), socket);
    }
}
