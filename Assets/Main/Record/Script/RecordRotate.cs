using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordRotate : MonoBehaviour
{
    public float rotatespeed;

    
    void Update()
    {
        transform.Rotate(Vector3.down * (rotatespeed * Time.deltaTime));
    }
}
