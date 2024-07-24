using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class ObsPlayerControl : MonoBehaviourPun
{
    public Animator ani;
    
    
    private static readonly int UpA = Animator.StringToHash("UpA");
    private static readonly int DownA = Animator.StringToHash("DownA");
    private static readonly int LeftA = Animator.StringToHash("LeftA");
    private static readonly int RightA = Animator.StringToHash("RightA");

    void Start()
    {
        ani.GetComponent<Animator>();
    }

    public void UPMotion(InputAction.CallbackContext context)
    {


        if (context.started && photonView.IsMine)
        {
            ani.SetTrigger(UpA);
        }


    }

    public void DownMotion(InputAction.CallbackContext context)
    {

        if (context.started && photonView.IsMine)
        {
            ani.SetTrigger(DownA);
        }
        

    }

    public void LeftMotion(InputAction.CallbackContext context)
    {

        if (context.started && photonView.IsMine)
        {
            ani.SetTrigger(LeftA);
        }
        

    }

    public void RightMotion(InputAction.CallbackContext context)
    {
        
        if (context.started && photonView.IsMine)
        {
            ani.SetTrigger(RightA);
        }
        
    }
}
