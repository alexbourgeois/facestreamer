using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[Serializable]
public class MeshData
{
    public SerializedVector3 pos;
    public SerializedVector3 rot;
    public SerializedVector3 scale;

    [SerializeField] public int[] triangles;
    public SerializedVector3[] vertices;
    public SerializedVector2[] uv;
    //public SerializedVector3[] normals;

    public static void ConvertToSerialized3(ref SerializedVector3 output, Vector3 vector)
    {
        output.x = vector.x;
        output.y = vector.y;
        output.z = vector.z;
    }

    public static void ConvertToSerialized2(ref SerializedVector2 output, Vector2 vector)
    {
        output.x = vector.x;
        output.y = vector.y;
    }

    public static void ConvertFromSerialized3(ref SerializedVector3 vector, ref Vector3 output)
    {
        output.x = vector.x;
        output.y = vector.y;
        output.z = vector.z;
    }

    public static void ConvertFromSerialized2(ref SerializedVector2 vector, ref Vector2 output)
    {
        output.x = vector.x;
        output.y = vector.y;
    }

    public static void ConvertToSerialized3Array(ref SerializedVector3[] output, Vector3[] vector)
    {
        for (var i = 0; i < vector.Length; i++)
        {
            ConvertToSerialized3(ref output[i], vector[i]);
        }
    }

    public static void ConvertFromSerialized3Array(ref Vector3[] output, SerializedVector3[] vector)
    {
        for (var i = 0; i < vector.Length; i++)
        {
            ConvertFromSerialized3(ref vector[i], ref output[i]);
        }
    }

    public static void ConvertToSerialized2Array(ref SerializedVector2[] output, Vector2[] vector)
    {
        for (var i = 0; i < vector.Length; i++)
        {
            ConvertToSerialized2(ref output[i], vector[i]);
        }
    }

    public static void ConvertFromSerialized2Array(ref Vector2[] output, SerializedVector2[] vector)
    {
        for (var i = 0; i < vector.Length; i++)
        {
            ConvertFromSerialized2(ref vector[i], ref output[i]);
        }
    }
}

[Serializable]
public class SerializedVector2
{
    [SerializeField] public float x;
    [SerializeField] public float y;
}
[Serializable]
public class SerializedVector3
{
    [SerializeField] public float x;
    [SerializeField] public float y;
    [SerializeField] public float z;
}

