using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    void Start()
    {
        MeshStreamer.instance.meshFilter = this.GetComponent<MeshFilter>();
        FaceInformationExtractor.instance.faceMeshFilter = this.GetComponent<MeshFilter>();
    }
}
