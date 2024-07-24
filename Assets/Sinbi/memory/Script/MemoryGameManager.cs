using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class MemoryGameManager : WholeGameManager
{
    public static MemoryGameManager instance;

    public TurnInfo[] turnDB;

    public Animator cpuAni;
    private List<int> randomMotions = new();
    private int turn = 0;

    protected static readonly int IsWMove = Animator.StringToHash("isWMove");
    protected static readonly int IsAMove = Animator.StringToHash("isAMove");
    protected static readonly int IsSMove = Animator.StringToHash("isSMove");
    protected static readonly int IsDMove = Animator.StringToHash("isDMove");

    private void Awake()
    {
        instance = this;
    }

    public override void GameStart()
    {
        MemoryPlayerController.instance.StartGame();
    }

    public override void GetScore()
    {

    }

    public override void GameEnd()
    {
        TotalManager.instance.ScoreBoardTest();
    }

    //입력을 받아서 누를 때마다 턴이 넘어가디록... 한 턴에 대한 모션 시퀀스 출력

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (turn >= turnDB.Length) return;
            
            RandomMotion();

            foreach (var num in randomMotions)
            {
                Debug.Log(num);
            }
            Debug.Log("----");
            turn++;
        }
    }

    public void RandomMotion()
    {
        randomMotions.Clear();
        for (int i = 0; i < turnDB[turn].numOfSeq; i++)
        {
            randomMotions.Add(Random.Range(0, 4));
        }
    }
}

[Serializable]
public class TurnInfo
{
    public int numOfSeq;
    public float motionPlayTime;
    
}

