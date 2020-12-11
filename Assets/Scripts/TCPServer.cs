using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;
using System.Text;
using System.IO;
using System.Collections.Concurrent;
using ProtoBuf;

public class TCPServer : MonoBehaviour
{
    public string ip;
    public int port;

    public TcpListener server;
    public TcpClient client;
    public NetworkStream stream;
    private Thread serverThread;
    public bool run;

    byte[] dataBuffer = new byte[0];
    public static event Action<MeshData> MsgReceived;

    private BinaryFormatter fmt = new BinaryFormatter();
    public ConcurrentQueue<MeshData> meshDatas = new ConcurrentQueue<MeshData>();

    MeshData tmpMeshData;

    void Start()
    {
        serverThread = new Thread(new ThreadStart(ServerMain));
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    public static string GetLocalIPAddress()
    {
        var host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (var ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                return ip.ToString();
            }
        }
        throw new Exception("No network adapters with an IPv4 address in the system!");
    }
    private void ServerMain()
    {
        ip = GetLocalIPAddress();
        server = new TcpListener(IPAddress.Parse(ip), port);
        server.Start();

        byte[] msgStart = new byte[] { 0x01, 0x02, 0x03, 0x04 };
        byte[] msgEnd = new byte[] { 0x04, 0x03, 0x02, 0x01 };

        byte[] shortMessageBuffer = new byte[4];
        var rcvdStart = false;
        int dataRead = 0;

        while (run)
        {
            if (client == null)
            {
                client = server.AcceptTcpClient();
                stream = client.GetStream();
                Debug.Log("Client connected!");
            }
            else
            {

                if (!client.Connected)
                {
                    Disconnect();
                    break;
                }

                dataRead = 0;

                rcvdStart = false;

                while (!rcvdStart && (dataRead = stream.Read(shortMessageBuffer, 0, 4)) != 0)
                {
                    rcvdStart = true;
                    for (int i=0; i< 4;i++)
                    {
                        if (shortMessageBuffer[i] != msgStart[i]) 
                            rcvdStart = false;
                    }
                }
                //Debug.Log("Received start!");
                dataRead = 0;
                
                int serializedDataSize = 0;

                while ((dataRead = stream.Read(shortMessageBuffer, 0, 4)) != 0)
                {
                    if (dataRead == 4)
                    {
                        serializedDataSize = BitConverter.ToInt32(shortMessageBuffer, 0);
                        break;
                    }
                }
               // Debug.Log("Size : " + serializedDataSize);
                dataRead = 0;
                dataBuffer = new byte[serializedDataSize];
                
                do
                {
                    dataRead += stream.Read(dataBuffer, dataRead, serializedDataSize - dataRead);

                } while (dataRead < serializedDataSize);
               // Debug.Log("Received all data");
                dataRead = 0;
                var rcvdEnd = false;
                while (!rcvdEnd && (dataRead = stream.Read(shortMessageBuffer, 0, 4)) != 0)
                {
                    rcvdEnd = true;
                    for (int i = 0; i < 4; i++)
                    {
                        if (shortMessageBuffer[i] != msgEnd[i])
                            rcvdEnd = false;
                    }
                }
                //Debug.Log("Received end message.");

                if (dataBuffer.Length != 256)
                {
                    var ms = new MemoryStream(dataBuffer);
                    ms.Position = 0;
                    if(ms.Length == 0)
                    {
                        Disconnect();
                    } 
                    MeshData md;
                    try
                    {
                        //Debug.Log("Deserializing " + ms.Length);
                        md = Serializer.Deserialize<MeshData>(ms);
                        //md = (MeshData)fmt.Deserialize(ms);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError("Error deserializing : " + e.Message);
                        //Disconnect();
                        continue;
                    }

                    meshDatas.Enqueue(md);
                }
             
            }
        }
    }

    void Disconnect()
    {
        stream.Close();
        client = null;
        stream = null;
        Debug.Log("Client disconnected.");
    }
    // Update is called once per frame
    void Update()
    { 
        if (meshDatas.TryDequeue(out tmpMeshData))
        {
            MsgReceived?.Invoke(tmpMeshData);
        }

    }
}
