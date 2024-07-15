using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerFTM : WholeGameManager
{
    public static GameManagerFTM instance;

    private void Awake()
    {
        instance = this;
    }

    public override void GameStart()
    {
        RandomMotion.instance.StartGame();
    }

    public override void GetScore()
    {

    }

    public override void GameEnd()
    {
        TotalManager.instance.ScoreBoardTest();
    }
}
