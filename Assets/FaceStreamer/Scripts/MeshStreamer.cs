using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;
using ProtoBuf;

public class MeshStreamer : MonoBehaviour
{
    public static MeshStreamer instance;

    public MeshFilter meshFilter;

    public TCPSocket socket;
    private MemoryStream mem = new MemoryStream();
    private MeshData meshData;

    byte[] msgStart = new byte[] { 0x01, 0x02, 0x03, 0x04 };
    byte[] msgEnd = new byte[] { 0x04, 0x03, 0x02, 0x01 };

    [HideInInspector]
    public bool ready = false;

    private void Update()
    {
        if (meshFilter != null && socket.client.Connected && UIManager.instance.tcpEnabled)
        {
            if (meshData == null)
            {
                meshData = new MeshData();
                meshData.pos = new SerializedVector3();
                meshData.rot = new SerializedVector3();
                meshData.scale = new SerializedVector3();
                /*meshData.normals = new SerializedVector3[meshFilter.mesh.normals.Length];
                for (var i = 0; i < meshFilter.mesh.normals.Length; i++)
                    meshData.normals[i] = new SerializedVector3();*/

                meshData.vertices = new SerializedVector3[meshFilter.sharedMesh.vertices.Length];
                for (var i = 0; i < meshFilter.sharedMesh.vertices.Length; i++)
                    meshData.vertices[i] = new SerializedVector3();

                meshData.uv = new SerializedVector2[meshFilter.sharedMesh.uv.Length];
                for (var i = 0; i < meshFilter.sharedMesh.uv.Length; i++)
                    meshData.uv[i] = new SerializedVector2();

                meshData.triangles = new int[meshFilter.sharedMesh.triangles.Length];
                ready = true;
            }

           // MeshData.ConvertToSerialized3Array(ref meshData.normals, meshFilter.mesh.normals);
            MeshData.ConvertToSerialized3Array(ref meshData.vertices, meshFilter.sharedMesh.vertices);
            meshData.triangles = meshFilter.sharedMesh.triangles;
            MeshData.ConvertToSerialized2Array(ref meshData.uv, meshFilter.sharedMesh.uv);
            MeshData.ConvertToSerialized3(ref meshData.pos, meshFilter.transform.position);
            MeshData.ConvertToSerialized3(ref meshData.rot, meshFilter.transform.rotation.eulerAngles);
            MeshData.ConvertToSerialized3(ref meshData.scale, meshFilter.transform.localScale);

            /*meshData.normals = MeshData.ConvertToSerialized3Array(meshFilter.mesh.normals);
            meshData.vertices = MeshData.ConvertToSerialized3Array(meshFilter.mesh.vertices);
            meshData.uv2 = MeshData.ConvertToSerialized2Array(meshFilter.mesh.uv2);*/

            SendMesh("Face", meshData);

        }
    }

    private void Awake()
    {
        instance = this;
    }
    
    public void SendMesh(string name, MeshData data)
    {
        mem.Seek(0, SeekOrigin.Begin);
        Serializer.Serialize(mem, data);
        mem.Seek(0, SeekOrigin.Begin);
        socket.SendMessage(msgStart);
        //Debug.LogError("Length : " + mem.Length);
        var memSize = BitConverter.GetBytes((Int32)mem.Length);

        var tmp = new byte[] { memSize[0], memSize[1], memSize[2], memSize[3] };
        socket.SendMessage(tmp);
        socket.SendMessage(mem.GetBuffer());
        socket.SendMessage(msgEnd);
        //Debug.Log("Done!");
    }

}