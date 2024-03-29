﻿using UnityEngine;
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
    private MeshData meshData;

    private Vector3[] myVertices = new Vector3[0];
    private int[] myTriangles = new int[0];
    private Vector2[] myUv = new Vector2[0];

    private Vector3 myPosition = new Vector3();
    private Vector3 myRotation = new Vector3();
    private Vector3 myScale = new Vector3();

    private Mesh actualMesh;
    private Mesh targetMesh;

    public float mostRecentMeshStickyness = 35.0f;
    [HideInInspector]
    public bool ready = false;

    private Vector3[] tempActualVertices;
    private Vector3[] tempTargetVertices;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        TCPServer.MsgReceived += SetMesh;
        targetMesh = new Mesh();
    }

    void Update()
    {
       if (ready) {
            if(actualMesh == null)
            {
                actualMesh = (Mesh)Instantiate(targetMesh);
                if(FaceInformationExtractor.instance != null)
                {
                    FaceInformationExtractor.instance.faceMeshFilter = meshFilter;
                }
            }
            tempActualVertices = actualMesh.vertices;
            tempTargetVertices = targetMesh.vertices;
            for (int i=0;i< tempActualVertices.Length;i++)
            {
                tempActualVertices[i] = Vector3.Lerp(tempActualVertices[i], tempTargetVertices[i], Time.deltaTime * mostRecentMeshStickyness);
            }
            actualMesh.vertices = tempActualVertices;
            /*for (int i = 0; i < actualMesh.normals.Length; i++)
            {
                actualMesh.normals[i] = Vector3.Lerp(actualMesh.normals[i], targetMesh.normals[i], Time.deltaTime * strength);
            }*/

            actualMesh.RecalculateNormals();
            actualMesh.RecalculateBounds();

            meshFilter.transform.localPosition = Vector3.Lerp(meshFilter.transform.localPosition, myPosition * meshFilter.transform.localScale.y, Time.deltaTime * mostRecentMeshStickyness);
            meshFilter.transform.localRotation = Quaternion.Lerp(meshFilter.transform.localRotation, Quaternion.Euler(myRotation), Time.deltaTime * mostRecentMeshStickyness);
           
            meshFilter.sharedMesh = actualMesh;
        }
    }

    public void SetMesh(MeshData data)
    {
        meshData = data;

        if (meshData == null)
        {
            Debug.Log("[MeshReceiver] No mesh data.");
            return;
        }

        if(meshData.vertices == null)
        {
            Debug.Log("[MeshReceiver] No vertex data.");
            return;
        }

        myVertices = new Vector3[meshData.vertices.Length];
        myTriangles = new int[meshData.triangles.Length];
        myUv = new Vector2[meshData.uv.Length];

        myPosition = new Vector3();
        myRotation = new Vector3();
        myScale = new Vector3();

        MeshData.ConvertFromSerialized3(ref meshData.pos, ref myPosition);
        MeshData.ConvertFromSerialized3(ref meshData.rot, ref myRotation);
        MeshData.ConvertFromSerialized3(ref meshData.scale, ref myScale);

        //myNormals = new Vector3[meshData.normals.Length];

        MeshData.ConvertFromSerialized3Array(ref myVertices, meshData.vertices);
        //MeshData.ConvertFromSerialized3Array(ref myNormals, meshData.normals);
        MeshData.ConvertFromSerialized2Array(ref myUv, meshData.uv);

        myTriangles = meshData.triangles;
        myPosition.x *= 1.0f; //mirror


        targetMesh.vertices = myVertices;
        targetMesh.triangles = myTriangles;
        targetMesh.uv = myUv;
        //mesh.normals = myNormals;

        targetMesh.RecalculateNormals();
        targetMesh.RecalculateBounds();

        ready = true;
    }
}
