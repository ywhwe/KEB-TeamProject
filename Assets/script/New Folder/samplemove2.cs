using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class samplemove2 : MonoBehaviour
{
    public float movespeed=3.33f;
    private Rigidbody rigid;
    public float power;
    private bool go = true;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        go = true;
    }


    void Update()
    {
        if (go)
        {
            transform.Translate(movespeed*Time.deltaTime,0f,0f);
        } 
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6)
        {
            rigid.AddForce(0f, 0f, power, ForceMode.Impulse);
            go = false;
        }
    }
}
