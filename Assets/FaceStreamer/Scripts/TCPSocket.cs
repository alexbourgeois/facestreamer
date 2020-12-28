using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEngine.UI;
using System;
using System.Linq;

public class TCPSocket : MonoBehaviour
{
    public InputField ipText;
    public InputField portText;

    public Text connectionStatus;
    public string ip;
    public int port;

    public TcpClient client;
    public NetworkStream clientStream;

    public float connectionDelay = 2.0f;
    private float _previousConnectionTime;

    void Start()
    {
        if (PlayerPrefs.HasKey("TCP-IP"))
        {
            ip = PlayerPrefs.GetString("TCP-IP");
            ipText.text = ip;
        }
        if (PlayerPrefs.HasKey("TCP-PORT"))
        {
            port = PlayerPrefs.GetInt("TCP-PORT");
            portText.text = port.ToString();
        }
        client = new TcpClient();
    }

    public void NewIP(string _ip)
    {
        if (!ValidateIPv4(_ip))
            return;

        ip = _ip;
        PlayerPrefs.SetString("TCP-IP", ip);

        Connect();
    }

    public bool ValidateIPv4(string ipString)
    {
        if (String.IsNullOrWhiteSpace(ipString))
        {
            return false;
        }

        string[] splitValues = ipString.Split('.');
        if (splitValues.Length != 4)
        {
            return false;
        }

        byte tempForParsing;

        return splitValues.All(r => byte.TryParse(r, out tempForParsing));
    }

    public void NewPort(string _port)
    {
        try
        {
            port = Int32.Parse(_port);
        }
        catch (Exception)
        {
            return;
        }

        PlayerPrefs.SetInt("TCP-PORT", port);

        Connect();
    }

    public void Connect()
    {
        if (!UIManager.instance.tcpEnabled)
            return;

        Debug.Log("[TCPSocket] Connecting to : " + ip + ":" + port);
        try
        {
            if(client.Connected)
            {
                if (clientStream != null)
                    clientStream.Close();
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
            Debug.LogWarning("Failed : " + e.Message);
            _previousConnectionTime = Time.time;
            if(clientStream != null)
                clientStream.Close();
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
        try { 
            clientStream.Write(bytes, 0, bytes.Length);
        }
        catch (Exception)
        {
            _previousConnectionTime = Time.time;
        }
    }

    void Update()
    {
        if (client.Connected)
        {
            connectionStatus.text = "Yes";
        }
        else
        {
            connectionStatus.text = "No";
            if (Time.time - _previousConnectionTime >= connectionDelay)
                Connect();
        }

    }
}
