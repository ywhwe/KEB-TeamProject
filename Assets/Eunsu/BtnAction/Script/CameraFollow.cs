using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using Vector2 = UnityEngine.Vector2;

public class CameraFollow : MonoBehaviour
{
    private Transform target;
    private Vector3 positionOffset = new (-1f, 2f, -2f);
    private Vector3 velocity = Vector3.zero;
    private float smoothTime = 0.5f;

    private void Start()
    {
        target = GameObject.FindWithTag("Trolley").transform;
    }

    private void Update()
    {
        var targetPosition = target.position + positionOffset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
