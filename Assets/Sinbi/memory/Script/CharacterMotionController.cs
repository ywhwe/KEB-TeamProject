using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotionController : MonoBehaviour
{
    public static CharacterMotionController instance;
    
    private Animator ani;

    private bool isInputActive = true;
    
    public bool isTwoKey = false;
    
    private KeyCode[] key =
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
        KeyCode.W,
        KeyCode.S
    };
    
    private int[] motionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isAMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isDMove"),
        Animator.StringToHash("isTwoKeyWMove"),
        Animator.StringToHash("isTwoKeySMove")
    };
    
    public event Action<int> OnKeyPressed;
    
    void Awake()
    {
        instance = this;
        ani = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (!isInputActive) return;

        if (!isTwoKey)
        {
            for (int i = 0; i <= 3; i++)
            {
                if (Input.GetKeyDown(key[i]))
                {
                    ani.SetTrigger(motionHash[i]);
                    OnKeyPressed?.Invoke(i);
                }
            }
        }
        
        if (isTwoKey)
        {
            for (int i = 4; i <= 5; i++)
            {
                if (Input.GetKeyDown(key[i]))
                {
                    ani.SetTrigger(motionHash[i]);
                    OnKeyPressed?.Invoke(i);
                }
            }
        }
        

        
        
    }

    public void SetActiveInput(bool active)
    {
        isInputActive = active;
    }
}
