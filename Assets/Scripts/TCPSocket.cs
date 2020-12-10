using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;
using System;

public class TCPSocket : MonoBehaviour
{
    public Text text;
    public string ip;
    public int port;

    public TcpClient client;
    public NetworkStream clientStream;

    public float connectionDelay = 2.0f;
    private float _previousConnectionTime;

    void Start()
    {
        client = new TcpClient();
    }

    public void Connect(string ipAndPort)
    {
        var strs = ipAndPort.Split(':');
        if(strs.Length != 2)
        {
            return;
        }
        ip = strs[0];
        port = Int32.Parse(strs[1]);
        Connect();
    }

    public void Connect()
    {
        try
        {
            client.Connect(ip, port);
            clientStream = client.GetStream();
        }
        catch(Exception e)
        {
            _previousConnectionTime = Time.time;
        }
        _previousConnectionTime = Time.time;
    }

    public void SendMessage(byte[] bytes)
    {
        if(!client.Connected)
        {
            return;
        }
        clientStream.Write(bytes, 0, bytes.Length);
    }

    void Update()
    {
        if (client.Connected)
        {
            text.text = "Connected : True";
        }
        else
        {
            text.text = "Connected : False";
            if (Time.time - _previousConnectionTime >= connectionDelay)
                Connect();
        }

    }
}
