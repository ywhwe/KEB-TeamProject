using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trolley : MonoBehaviour
{
    [SerializeField] private Animator ani;
    
    [SerializeField] private GameObject FrontWheels;
    [SerializeField] private GameObject BackWheels;

    private static readonly int isFinished = Animator.StringToHash("isFinished");

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        ObjMover.ObjInstance.frontWheels = FrontWheels;
        ObjMover.ObjInstance.backWheels = BackWheels;
    }

    private void Update()
    {
        if (!GameManagerBtn.instance.flag) ani.SetBool(isFinished, true);
    }
}
