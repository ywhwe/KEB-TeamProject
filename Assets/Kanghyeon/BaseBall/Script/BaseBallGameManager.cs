using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BaseBallGameManager : WholeGameManager
{
    
    public static BaseBallGameManager instance;
    public TextMeshProUGUI finishboard;
    public PitcherSquid pitcher;
    public Bat bat;
    // public float score;
    public float starttime;
    public int balltotal;
    public int ballcount;
    public int finalscore;
    
    public bool IsGameStart=false;
    public bool IsGameEnd=false;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GameStart();
    }

    public override void GameStart()
    {
        starttime = Time.time;
        pitcher.ShootBall();
        bat.IsGameStart();
    }

    public override void GetScore()
    {
        score = finalscore;
    }

    public override void GameEnd()
    {
        bat.IsGameEnd();
    }
    
    public void CountBall()
    {
        ballcount++;
        if (ballcount == balltotal)
        {
            IsGameEnd = true;
            finishboard.text = "End"+" Score:"+finalscore;
            
        }
    }

    public void CountScore()
    {
        finalscore++;
        CountBall();
    }
}
