using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInput : MonoBehaviour
{
    private InputField Field;
    // Start is called before the first frame update
    void Start()
    {
        Field = GetComponent<InputField>();
    }

    public void ReadIP(string s)
    {
       ClientTCP.startClient(s);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
