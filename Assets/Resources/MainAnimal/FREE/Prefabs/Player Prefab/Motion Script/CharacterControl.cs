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
    private static readonly int IsTwoKeyWMove = Animator.StringToHash("isTwoKeyWMove");
    private static readonly int IsTwoKeySMove = Animator.StringToHash("isTwoKeySMove");

    public static bool isTwoKey = false;
    
    void Start()
    {
        ani.GetComponent<Animator>();
    }

    public void WMotion(InputAction.CallbackContext context)
    {
        if (!isTwoKey)
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
        
        if (isTwoKey)
        {
            if (context.started)
            {
                ani.SetBool(IsTwoKeyWMove, true);
            }

            if (context.canceled)
            {
                ani.SetBool(IsTwoKeyWMove, false);
            }
        }
    }
    
    public void AMotion(InputAction.CallbackContext context)
    {
        if (!isTwoKey)
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
    }
    
    public void SMotion(InputAction.CallbackContext context)
    {
        if (!isTwoKey)
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
        if (isTwoKey)
        {
            if (context.started)
            {
                ani.SetBool(IsTwoKeySMove, true);
            }

            if (context.canceled)
            {
                ani.SetBool(IsTwoKeySMove, false);
            }
        }
    }
    
    public void DMotion(InputAction.CallbackContext context)
    {
        if (!isTwoKey)
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
}
