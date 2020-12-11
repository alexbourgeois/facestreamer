using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;
using System;

public class TCPSocket : MonoBehaviour
{
    public Text ipAndPortText;

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
        if (PlayerPrefs.HasKey("IP"))
        {
            ip = PlayerPrefs.GetString("IP");
            port = PlayerPrefs.GetInt("PORT");

            ipAndPortText.text = ip + ":" + port;
        } 
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
            if (client.ConnectAsync(ip, port).Wait(1000))
            {
                // connection failure
                clientStream = client.GetStream();
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Failed : " + e.Message);
            _previousConnectionTime = Time.time;
        }
        ipAndPortText.text = ip + ":" + port;
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
