using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class RandomMotion : MonoBehaviour
{
    public Animator ani;
    
    private static readonly int IsWMove = Animator.StringToHash("isWMove");
    private static readonly int IsAMove = Animator.StringToHash("isAMove");
    private static readonly int IsSMove = Animator.StringToHash("isSMove");
    private static readonly int IsDMove = Animator.StringToHash("isDMove");
    
    public int randomMotionNumber;
    private int stage = 0;
    private static float timeRate = 50f;
    
    public GameObject player1;
    public GameObject player2;
    private int player1Motion, player2Motion;
    private int player1Life, player2Life = 3;
    
    
    void Start()
    {
        ani.GetComponent<Animator>();
    }
    
    void Update()
    {
        RandomAction();
        for (float timer = 0f; timer < timeRate;)
        {
            Debug.Log(timer);
            timer += Time.deltaTime;
            player1Motion = player1.GetComponent<CharacterControl>().motionNumber;
            player2Motion = player2.GetComponent<CharacterControl>().motionNumber;

        }
            CompareMotionNumber(player1Motion, player1Life);
            CompareMotionNumber(player2Motion, player2Life);
            
            EndGame();
            
            stage++;
            if (stage >= 5)
            {
                Time.timeScale += 0.5f;
                stage = 0;
            }
    }

    public void RandomAction()
    {
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


    }

    public int CompareMotionNumber(int playerMotionNumber, int playerLife)
    {
        if (playerMotionNumber != randomMotionNumber)
        {
            playerLife--;
        }
        
        return playerLife;
    }

    public void EndGame()
    {
        if (player1Life == 0 || player2Life == 0)
        {
            Debug.Log("Game Over");
        }
    }
}
