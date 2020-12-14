using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceInformationExtractor : MonoBehaviour
{
    private int _noseTipindex = 4;

    private int _rightEyeIndex1 = 163;
    private int _rightEyeIndex2 = 157;

    private int _leftEyeIndex1 = 384;
    private int _leftEyeIndex2 = 390;

    private int _mouthIndex1 = 13;
    private int _mouthIndex2 = 14;

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
            if(meshReceiver.ready)
            {
                var vertices = meshReceiver.meshFilter.sharedMesh.vertices;
                noiseTip.position = meshReceiver.meshFilter.transform.TransformPoint(vertices[_noseTipindex]);
                noiseTip.rotation = meshReceiver.meshFilter.transform.localRotation;

                leftEye.position = meshReceiver.meshFilter.transform.TransformPoint((vertices[_leftEyeIndex1] + vertices[_leftEyeIndex2]) / 2.0f);
                rightEye.position = meshReceiver.meshFilter.transform.TransformPoint((vertices[_rightEyeIndex1] + vertices[_rightEyeIndex2]) / 2.0f);
                rightEye.rotation = meshReceiver.meshFilter.transform.localRotation;
                leftEye.rotation = meshReceiver.meshFilter.transform.localRotation;

                mouth.position = meshReceiver.meshFilter.transform.TransformPoint((vertices[_mouthIndex1] + vertices[_mouthIndex2]) / 2.0f);
                mouth.rotation = meshReceiver.meshFilter.transform.localRotation;

            }
        }
    }
}
