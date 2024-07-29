using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RandomMotion : MonoBehaviour
{
    public Animator ani;
    public static RandomMotion instance;
    
    public int randomMotionNumber;
    public int stage = 0;
    
    protected static readonly int IsWMove = Animator.StringToHash("isWMove");
    protected static readonly int IsAMove = Animator.StringToHash("isAMove");
    protected static readonly int IsSMove = Animator.StringToHash("isSMove");
    protected static readonly int IsDMove = Animator.StringToHash("isDMove");
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ani.GetComponent<Animator>();
        
    }
    
    public void RandomAction()
    {
        randomMotionNumber = Random.Range(0, 4);

        if (randomMotionNumber == 0)
        {
            ani.SetTrigger(IsWMove);
        }
        else if (randomMotionNumber == 1)
        {
            ani.SetTrigger(IsAMove);
        }
        else if (randomMotionNumber == 2)
        {
            ani.SetTrigger(IsSMove);
        }
        else if (randomMotionNumber == 3)
        {
            ani.SetTrigger(IsDMove);
        }
        
        Debug.Log(randomMotionNumber);
        stage++;
        if (stage >= 4)
        {
            Time.timeScale += 0.5f;
            stage = 0;
        }
    }
    
    public int CompareMotionNumber(int playerMotionNumber, int playerLife)
    {
        if (playerMotionNumber != randomMotionNumber)
        {
            playerLife--;
        }
        return playerLife;
    }
}
