using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceInformationExtractor : MonoBehaviour
{
    public static FaceInformationExtractor instance;

    private int _noseTipindex = 4;

    private int _rightEyeIndex1 = 163;
    private int _rightEyeIndex2 = 157;

    private int _leftEyeIndex1 = 384;
    private int _leftEyeIndex2 = 390;

    private int _topLipsIndex1 = 13;
    private int _botLipsIndex2 = 14;

    private int _mouthIndex1 = 62;
    private int _mouthIndex2 = 292;

    public MeshFilter faceMeshFilter;

    public Transform noiseTip;
    public Transform leftEye;
    public Transform rightEye;
    public Transform mouth;
    public Vector3 faceDirection;
    public Vector3 facePosition;
    public float mouthHeight;
    public float mouthWidth;

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(faceMeshFilter != null)
        {
            var vertices = faceMeshFilter.sharedMesh.vertices;
            noiseTip.position = faceMeshFilter.transform.TransformPoint(vertices[_noseTipindex]);
            noiseTip.rotation = faceMeshFilter.transform.localRotation;

            leftEye.position = faceMeshFilter.transform.TransformPoint((vertices[_leftEyeIndex1] + vertices[_leftEyeIndex2]) / 2.0f);
            rightEye.position = faceMeshFilter.transform.TransformPoint((vertices[_rightEyeIndex1] + vertices[_rightEyeIndex2]) / 2.0f);
            rightEye.rotation = faceMeshFilter.transform.localRotation;
            leftEye.rotation = faceMeshFilter.transform.localRotation;

            mouth.position = faceMeshFilter.transform.TransformPoint((vertices[_topLipsIndex1] + vertices[_botLipsIndex2]) / 2.0f);
            mouth.rotation = faceMeshFilter.transform.localRotation;

            mouthHeight = Vector3.Distance(vertices[_topLipsIndex1], vertices[_botLipsIndex2]);
            mouthWidth = Vector3.Distance(vertices[_mouthIndex1], vertices[_mouthIndex2]);

            faceDirection = faceMeshFilter.transform.forward;
            facePosition = faceMeshFilter.transform.localPosition;
            
        }
    }
}
