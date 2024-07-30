using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolley : MonoBehaviour
{
    public static Trolley trolleyInstance;
    private ObjMover mover;
    
    [SerializeField] private Animator ani;
    
    [SerializeField] private GameObject FrontWheels;
    [SerializeField] private GameObject BackWheels;

    public ParticleSystem accelerationSmoke;
    public ParticleSystem decelerationSpark1;
    public ParticleSystem decelerationSpark2;
    
    private static readonly int isFinished = Animator.StringToHash("isFinished");

    private void Awake()
    {
        trolleyInstance = this;
        
        ani = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        mover = ObjMover.ObjInstance;
    }

    private void Start()
    {
        ObjMover.ObjInstance.frontWheels = FrontWheels;
        ObjMover.ObjInstance.backWheels = BackWheels;
        mover.smoke = accelerationSmoke;
        mover.spark1 = decelerationSpark1;
        mover.spark2 = decelerationSpark2;
    }

    private void Update()
    {
        if (!GameManagerBtn.instance.flag) ani.SetBool(isFinished, true);
    }
}
