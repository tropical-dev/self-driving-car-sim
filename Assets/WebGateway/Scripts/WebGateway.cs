using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public class WebGatewayMessage
{
    public string type;
    public string data;
}

public class WebGateway : MonoBehaviour
{
#if UNITY_EDITOR
    private static void JsSend(string type, string data)
    {
        Debug.Log("Send: " + type + data);
    }
#elif UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void JsSend(string type, string data);
#endif

    public delegate void MessageReceivedHandler(string type, string data);
    public event MessageReceivedHandler MessageReceived;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("WebGateway init");
        //Receive(JsonUtility.FromJson<WebGatewayMessage>("{\"steering_angle\": 10, \"throttle\": 10}"));

    }

    void Receive(string msg_str)
    {
        var msg = JsonUtility.FromJson<WebGatewayMessage>(msg_str);
        //Debug.Log("Receive: " + msg.type + msg.data);
        MessageReceived(msg.type, msg.data);
    }

    public void Send(string type, string data)
    {
        JsSend(type, data);
    }
}


