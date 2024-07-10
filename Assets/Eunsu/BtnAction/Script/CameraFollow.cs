using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothness;
    private Transform targetObject;
    private Vector3 initialOffset;
    private Vector3 cameraPosition;

    private void Start()
    {
        targetObject = GameManagerBtn.instance.trolleyClone.GetComponent<Transform>();
        initialOffset = transform.position - targetObject.position;
    }

    private void FixedUpdate()
    {
        cameraPosition = targetObject.position + initialOffset;
        transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothness*Time.fixedDeltaTime);
    }
}
