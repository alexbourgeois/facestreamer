using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;
using System.Text;
using UnityEngine.UI;

public class MeshReceiver : MonoBehaviour
{
    public static MeshReceiver instance;
    public MeshFilter meshFilter;
    public MeshData meshData;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TCPServer.MsgReceived += GetMesh;
    }

    public void SetMesh()
    {
        if (meshData == null)
        {
            Debug.Log("No mesh data.");
            return;
        }

        if(meshData.vertices == null)
        {
            Debug.Log("No vertex data.");
            //return;
        }
        /*if (meshData.normals == null)
        {
            Debug.Log("No normals data.");
            //return;
        }*/
        if (meshData.triangles == null)
        {
            Debug.Log("No triangles data.");
            //return;
        }
        if (meshData.uv2 == null)
        {
            Debug.Log("No uv2 data.");
            //return;
        }

        var myVertices = new Vector3[meshData.vertices.Length];
        var myTriangles = new int[meshData.triangles.Length];
        var myUv2 = new Vector2[meshData.uv2.Length];

        var myPosition = new Vector3();
        var myRotation = new Vector3();
        var myScale = new Vector3();

        MeshData.ConvertFromSerialized3(ref meshData.pos, ref myPosition);
        MeshData.ConvertFromSerialized3(ref meshData.rot, ref myRotation);
        MeshData.ConvertFromSerialized3(ref meshData.scale, ref myScale);

        MeshData.ConvertToSerialized3(ref meshData.rot, meshFilter.transform.rotation.eulerAngles);
        MeshData.ConvertToSerialized3(ref meshData.scale, meshFilter.transform.localScale);
        //var myNormals = new Vector3[meshData.normals.Length];

        MeshData.ConvertFromSerialized3Array(ref myVertices, meshData.vertices);
        //MeshData.ConvertFromSerialized3Array(ref myNormals, meshData.normals);
        MeshData.ConvertFromSerialized2Array(ref myUv2, meshData.uv2);
        myTriangles = meshData.triangles;
        

        var mesh = new Mesh();
        mesh.vertices = myVertices;
        mesh.triangles = myTriangles;
        mesh.uv = myUv2;
        //mesh.normals = myNormals;


        mesh.RecalculateNormals();

        mesh.RecalculateBounds();
       
        meshFilter.mesh = mesh;
        meshFilter.transform.localPosition = myPosition;
        meshFilter.transform.localRotation = Quaternion.Euler(myRotation);
        //meshFilter.transform.localScale = myScale;
    }
    public void GetMesh(MeshData data)
    {
        meshData = data;
        SetMesh();
    }
}
