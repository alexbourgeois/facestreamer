using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceInformationExtractor : MonoBehaviour
{
    private int noseTipindex = 4;

    private int rightEyeIndex1 = 163;
    private int rightEyeIndex2 = 157;

    private int leftEyeIndex1 = 384;
    private int leftEyeIndex2 = 390;

    private int mouthIndex1 = 13;
    private int mouthIndex2 = 14;

    public MeshReceiver meshReceiver;

    public Transform noiseTip;
    public Transform leftEye;
    public Transform rightEye;
    public Transform mouth;

    // Update is called once per frame
    void Update()
    {
        if(meshReceiver != null)
        {
            if(meshReceiver.actualMesh != null )
            {
                var vertices = meshReceiver.actualMesh.vertices;
                noiseTip.position = meshReceiver.meshFilter.transform.TransformPoint(vertices[noseTipindex]);
                noiseTip.rotation = meshReceiver.meshFilter.transform.localRotation;

                leftEye.position = meshReceiver.meshFilter.transform.TransformPoint((vertices[leftEyeIndex1] + vertices[leftEyeIndex2]) / 2.0f);
                rightEye.position = meshReceiver.meshFilter.transform.TransformPoint((vertices[rightEyeIndex1] + vertices[rightEyeIndex2]) / 2.0f);
                rightEye.rotation = meshReceiver.meshFilter.transform.localRotation;
                leftEye.rotation = meshReceiver.meshFilter.transform.localRotation;

                mouth.position = meshReceiver.meshFilter.transform.TransformPoint((vertices[mouthIndex1] + vertices[mouthIndex2]) / 2.0f);
                mouth.rotation = meshReceiver.meshFilter.transform.localRotation;

            }
        }
    }
}
