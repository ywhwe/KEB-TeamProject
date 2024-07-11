using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBall : MonoBehaviour
{
    private Rigidbody rigid;
    public float endtime;
    public float ballspeed=10f;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        rigid.AddForce(ballspeed,0f,0f,ForceMode.VelocityChange);
        
    }
    
    void Update()
    {
        
    }

    public void Init(float time)
    {
        endtime = time;
    }
}
