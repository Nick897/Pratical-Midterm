using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using UnityEditor.PackageManager;

public class MidtermClientUDP : MonoBehaviour
{
    //public GameObject myCube;
    private static byte[] buffer = new byte[4096];
    // the server end point
    private static IPEndPoint remoteEP;
    private static Socket clientSoc;

    private static byte[] bpos;
    private static float[] pos;
    // Identifyer should be 0 in client 1 & 1 in Client 2
    private static float identifyer = 0;

    private Vector3 cubeLastPos;

    public static void StartClient()
    {
        try
        {
            //IPAddress ip = IPAddress.Parse("192.168.2.178");
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            remoteEP = new IPEndPoint(ip, 8889);

            clientSoc = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // this is Async not needed rn
            //clientSoc.BeginReceive(bpos, 0, bpos.Length, 0, new AsyncCallback(RecieveCallback), clientSoc);
        }
        catch (Exception e)
        {
            Debug.Log("Exception: " + e.ToString());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        // find the cube in the scene
        //myCube = GameObject.Find("Cube");
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
    }
    public void SendCubePos()
    {
        pos = new float[] { gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z, identifyer };
        bpos = new byte[pos.Length * 4];
        // source with offset, destination with offset and length
        Buffer.BlockCopy(pos, 0, bpos, 0, bpos.Length);

        clientSoc.SendTo(bpos, remoteEP);
    }
    
}
