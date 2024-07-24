using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotionController : MonoBehaviour
{
    private Animator ani;

    private bool isInputActive = false;
    
    private KeyCode[] key =
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D
    };
    
    private int[] motionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isAMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isDMove")
    };
    
    public event Action<int> OnKeyPressed;
    
    void Awake()
    {
        ani = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!isInputActive) return;
        
        for (var i = 0; i < key.Length; i++)
        {
            if (Input.GetKeyDown(key[i]))
            {
                ani.SetTrigger(motionHash[i]);
                OnKeyPressed?.Invoke(i);
            }
        }
    }

    public void SetActiveInput(bool active)
    {
        isInputActive = active;
    }
}
