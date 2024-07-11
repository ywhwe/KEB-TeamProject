using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public int balltotal;
    public int ballcount;
    public int finalscore;

    public bool IsGameStart=false;
    public bool IsGameEnd=false;

    public TextMeshProUGUI finishboard;
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
