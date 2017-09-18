using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyBoxCamera : MonoBehaviour {

    public Camera MainCamera;
    public Camera SkyCamera;
    Vector3 SkyBoxRotation;


    void Start()
    {
        if (SkyCamera.depth >= MainCamera.depth)
        {
            Debug.Log("Set skybox camera depth lower " +
                " than main camera depth in inspector");
        }
        if (MainCamera.clearFlags != CameraClearFlags.Nothing)
        {
            Debug.Log("Main camera needs to be set to dont clear" +
                "in the inspector");
        }
        SkyBoxRotation = new Vector3(1.0f, 1.0f, 0);
    }

 
    void Update()
    {        
        SkyCamera.transform.Rotate(Vector3.up * Time.deltaTime);
    }
}
