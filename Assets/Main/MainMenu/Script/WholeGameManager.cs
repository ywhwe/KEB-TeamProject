using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WholeGameManager : MonoBehaviour
{
    public float score;
    public bool isGameEnd;
    
    
    
    public abstract void GameStart();

    public abstract void GetScore();
    
    public abstract void GameEnd();
}
