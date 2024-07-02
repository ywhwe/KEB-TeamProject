using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RunManager : MonoBehaviour
{
    public Score score;
    public TextMeshProUGUI scoreCounter;

    public void ScoreBoard()
    {
        scoreCounter.text = score.score.ToString();
    }
}
