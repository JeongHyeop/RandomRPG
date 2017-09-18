using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour {
    public Vector3 axis;
    public float speed = 0;
	void Start () {
		
	}
	void Update () {
        this.transform.Rotate(axis * speed * Time.deltaTime);
	}
}
