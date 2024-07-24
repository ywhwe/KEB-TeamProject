using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;


public class MemoryGameManager : WholeGameManager
{
    public static MemoryGameManager instance;

    public TurnInfo[] turnDB;
    public CharacterMotionController playerController;
    public Animator cpuAni;
    
    private List<int> randomMotions = new();
    private int playerInputIdx = 0;
    private int turn = 0; 
    private bool isPlayerTurn;

    private Coroutine cpuMotionPlayCoroutine;

    private float startTime=0;
    private float PlayingTime = 0;
    private float limitTime = 0;
  
    private int[] motionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isAMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isDMove")
    };
    protected static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
    
    private void Awake()
    {
        instance = this;
        playerController.OnKeyPressed += PlayerInput;
        StartGame();
    }

    private void Update()
    {
        //if ( time out )
        {
            if (isPlayerTurn)
            {
                //플레이어 입력 처리
                //스코어 계산
            }
            else
            {
                StopCoroutine(cpuMotionPlayCoroutine);
                //스코어 계산
            }
        }
    }
    
    public override void GameStart()
    {
        //StartGame();
    }

    public override void GetScore()
    {
        
    }

    public override void GameEnd()
    {
        TotalManager.instance.ScoreBoardTest();
    }


    private void StartGame()
    {
        //Timer 시작
        cpuMotionPlayCoroutine = StartCoroutine(PlayRandomMotion());
    }
    
    public void SelectRandomMotion()
    {
        randomMotions.Clear();
        for (int i = 0; i < turnDB[turn].numOfSeq; i++)
        {
            randomMotions.Add(Random.Range(0, 4));
        }
    }
    
    private IEnumerator PlayMotion()
    {
        for (int i = 0; i < randomMotions.Count; i++)
        {
            Debug.Log(randomMotions[i]);
            cpuAni.SetTrigger(motionHash[randomMotions[i]]);
            cpuAni.SetFloat(MotionSpeed, 1f/turnDB[turn].motionPlayTime);

            yield return new WaitForSeconds(turnDB[turn].motionPlayTime);
        }
    }
    
    private IEnumerator PlayRandomMotion()
    {
        Debug.Log($"Current turn is {turn}");
        
        isPlayerTurn = false;
        playerController.SetActiveInput(false);
        
        SelectRandomMotion();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(PlayMotion());

        playerInputIdx = 0;
        playerController.SetActiveInput(true);
        isPlayerTurn = true;
    }
    
    private void PlayerInput(int motionIdx)
    {
        if (!isPlayerTurn) return;

        if (randomMotions[playerInputIdx] == motionIdx)
        {
            //나아중에 이펙트 재생 (후순위)
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Incorrect");
            StartCoroutine(PlayRandomMotion());
        }
        
        playerInputIdx++;

        if (playerInputIdx == randomMotions.Count)
        {
            turn++;

            if (turn >= turnDB.Length)
            {
                CalcScore();
            }
            else
            {
                StartCoroutine(PlayRandomMotion());
            }
        }
    }

    private void CalcScore()
    {
        //게임하는 시간 기록, 제한시간 내에 못 끝내면 turn수가 점수...
        //시간이 끝났을 때 강제 종료
       
        
        
    }
}

[Serializable]
public class TurnInfo
{
    public int numOfSeq;
    public float motionPlayTime;
}