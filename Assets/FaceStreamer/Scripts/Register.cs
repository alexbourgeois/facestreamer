using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {
        MeshStreamer.instance.meshFilter = this.GetComponent<MeshFilter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
