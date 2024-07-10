using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControl : MonoBehaviour
{
    public Animator ani;
    public int motionNumber = 0;
    
    private static readonly int IsWMove = Animator.StringToHash("isWMove");
    private static readonly int IsAMove = Animator.StringToHash("isAMove");
    private static readonly int IsSMove = Animator.StringToHash("isSMove");
    private static readonly int IsDMove = Animator.StringToHash("isDMove");
    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    {
        ani.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void WMotion(InputAction.CallbackContext context)
    {
        motionNumber = 1;
        if (context.started)
        {
            ani.SetBool(IsWMove, true);
        }
        
        if (context.canceled)
        {
            ani.SetBool(IsWMove, false);
        }
        
    }
    
    public void AMotion(InputAction.CallbackContext context)
    {
        motionNumber = 2;
        if (context.started)
        {
            ani.SetBool(IsAMove, true);
        }
        
        if (context.canceled)
        {
            ani.SetBool(IsAMove, false);
        }
    }
    
    public void SMotion(InputAction.CallbackContext context)
    {
        motionNumber = 3;
        if (context.started)
        {
            ani.SetBool(IsSMove, true);
        }
        
        if (context.canceled)
        {
            ani.SetBool(IsSMove, false);
        }
    }
    
    public void DMotion(InputAction.CallbackContext context)
    {
        motionNumber = 4;
        if (context.started)
        {
            ani.SetBool(IsDMove, true);
        }
        
        if (context.canceled)
        {
            ani.SetBool(IsDMove, false);
        }
    }
}