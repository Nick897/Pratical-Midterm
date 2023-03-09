using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ServerIP : MonoBehaviour
{
    public string serverIP;
    public GameObject inputText;
    public GameObject displayMsg;

    public void SetServerIP()
    {
        //Get Text from input field from user
        serverIP = inputText.GetComponent<Text>().text;

        displayMsg.GetComponent<Text>().text = "ServerIP: " + serverIP;
    }
}
