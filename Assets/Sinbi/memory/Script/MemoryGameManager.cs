using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGameManager : WholeGameManager
{
    public static MemoryGameManager instance;
    
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
    
}
