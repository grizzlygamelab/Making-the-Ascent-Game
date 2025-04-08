using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    private Vector3 _offset;
    
    // Start is called before the first frame update
    void Start()
    {
           _offset = transform.position - objectToFollow.position; 
    }

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        transform.position = new Vector3(objectToFollow.position.x, 0f, objectToFollow.position.z) + _offset;
    }
}
