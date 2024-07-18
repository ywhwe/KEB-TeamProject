using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class RandomMotion : MonoBehaviour
{
    public Animator ani;
    public static RandomMotion instance;
    
    protected static readonly int IsWMove = Animator.StringToHash("isWMove");
    protected static readonly int IsAMove = Animator.StringToHash("isAMove");
    protected static readonly int IsSMove = Animator.StringToHash("isSMove");
    protected static readonly int IsDMove = Animator.StringToHash("isDMove");
    
    public int randomMotionNumber;
    protected int stage = 0;
    protected WaitForSeconds calTime = new WaitForSeconds(5f);
    
    public GameObject player1;
    public GameObject player2;
    protected int player1Motion, player2Motion;
    public int player1Life = 3; 
    public int player2Life = 3;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        ani.GetComponent<Animator>();
    }
    
    void Update()
    {
        player2Motion = player2.GetComponent<CharacterControl>().motionNumber;
        player1Motion = player1.GetComponent<CharacterControl>().motionNumber;
    }
    public IEnumerator PlayGameRoutine()
    {
        RandomAction();
        
        yield return calTime;
        
        Debug.Log(player1Motion);
        Debug.Log(player2Motion);
        
        player1Life = CompareMotionNumber(player1Motion, player1Life);
        player2Life = CompareMotionNumber(player2Motion, player2Life);
        
        Debug.Log("life1 "+ player1Life + " life2 "+player2Life);
        
        if (player1Life == 0 || player2Life == 0)
        {
            EndGame();
        }
        else
        {
            StartCoroutine(PlayGameRoutine());   
        }
    }
    
    public void RandomAction()
    {
        player2.GetComponent<CharacterControl>().motionNumber = 0;
        player1.GetComponent<CharacterControl>().motionNumber = 0;
        randomMotionNumber = 0;
        randomMotionNumber = Random.Range(1, 5);

        if (randomMotionNumber == 1)
        {
            ani.SetTrigger(IsWMove);
        }
        else if (randomMotionNumber == 2)
        {
            ani.SetTrigger(IsAMove);
        }
        else if (randomMotionNumber == 3)
        {
            ani.SetTrigger(IsSMove);
        }
        else if (randomMotionNumber == 4)
        {
            ani.SetTrigger(IsDMove);
        }

        stage++;
        if (stage >= 5)
        {
            Time.timeScale += 0.5f;
            stage = 0;
        }
        Debug.Log(randomMotionNumber);
    }

    public int CompareMotionNumber(int playerMotionNumber, int playerLife)
    {
        if (playerMotionNumber != randomMotionNumber)
        {
            playerLife--;
        }
        return playerLife;
    }

    public void StartGame()
    {
        StartCoroutine(PlayGameRoutine());
    }

    public void EndGame()
    {
        Time.timeScale = 1f;
        stage = 0;
        GameManagerFTM.instance.GameEnd();
    }
}
