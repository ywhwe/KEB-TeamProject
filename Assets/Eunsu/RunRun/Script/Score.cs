using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public int score;
    public HitJudgement hit;

    private void Awake()
    {
        score = 0;
    }

    public void CountScore()
    {
        if (hit.isHit)
        {
            score += 100;
        }
    }

}
