using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class samplemove : MonoBehaviour
{
    public float movespeed=3.33f;
    void Start()
    {
        
    }


    void Update()
    {
        transform.Translate(0f,0f,movespeed*Time.deltaTime);
    }
}
