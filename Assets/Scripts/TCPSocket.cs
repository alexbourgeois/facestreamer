using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;
using System;

public class TCPSocket : MonoBehaviour
{
    public InputField ipAndPortText;

    public Text connectionStatus;
    public string ip;
    public int port;

    public TcpClient client;
    public NetworkStream clientStream;

    public float connectionDelay = 2.0f;
    private float _previousConnectionTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("IP"))
        {
            ip = PlayerPrefs.GetString("IP");
            port = PlayerPrefs.GetInt("PORT");

            ipAndPortText.text = ip + ":" + port;
        } 
        client = new TcpClient();
    }

    public void Connect(string ipAndPort)
    {
        var strs = ipAndPort.Split(':');
        if(strs.Length != 2)
        {
            return;
        }
        try
        {
            ip = strs[0];
            port = Int32.Parse(strs[1]);
        }
        catch(Exception e)
        {
            return;
        }
        PlayerPrefs.SetString("IP", ip);
        PlayerPrefs.SetInt("PORT", port);

        ipAndPortText.text = ip + ":" + port;
        Connect();
    }

    public void Connect()
    {
        Debug.Log("Connecting to : " + ip + ":" + port);
        try
        {
            if(client.Connected)
            {
                client.Close();
                client = new TcpClient();
            }
            if (client.ConnectAsync(ip, port).Wait(500))
            {
                clientStream = client.GetStream();
            }
        }
        catch(Exception e)
        {
            //Debug.LogError("Failed : " + e.Message);
            _previousConnectionTime = Time.time;
            client = new TcpClient();
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
            connectionStatus.text = "Connected : True";
        }
        else
        {
            connectionStatus.text = "Connected : False";
            if (Time.time - _previousConnectionTime >= connectionDelay)
                Connect();
        }

    }
}
