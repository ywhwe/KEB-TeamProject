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
    public bool isMirrored = false;
    
    private KeyCode[] keyCodes =
    {
        KeyCode.W,
        KeyCode.A,
        KeyCode.S,
        KeyCode.D,
    };
    
    private KeyCode[] twoKeyCodes =
    {
        KeyCode.W,
        KeyCode.S,
    };
    
    private int[] motionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isAMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isDMove")
    };
    
    private int[] mirroredMotionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isDMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isAMove")
    };

    private int[] twoKeyMotionHash =
    {
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

        var keyCode = isTwoKey ? twoKeyCodes : keyCodes;
        var hashCode = isTwoKey ? twoKeyMotionHash : (isMirrored ? mirroredMotionHash : motionHash);
        
        for (int i = 0; i < hashCode.Length; i++)
        {
            if (Input.GetKeyDown(keyCode[i]))
            {
                ani.SetTrigger(hashCode[i]);
                OnKeyPressed?.Invoke(i);
            }
        }
        
        // if (isTwoKey)
        // {
        //     for (int i = 0; i <= 1; i++)
        //     {
        //         if (Input.GetKeyDown(key[i]))
        //         {
        //             ani.SetTrigger(twoKeyMotionHash[i]);
        //             OnKeyPressed?.Invoke(i);
        //         }
        //     }
        // }
        // else
        // {
        //     for (int i = 0; i <= 3; i++)
        //     {
        //         if (Input.GetKeyDown(key[i]))
        //         {
        //             ani.SetTrigger(motionHash[i]);
        //             OnKeyPressed?.Invoke(i);
        //         }
        //     }
        // }
        
    }

    public void SetActiveInput(bool active)
    {
        isInputActive = active;
    }
}
