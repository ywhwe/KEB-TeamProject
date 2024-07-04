using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public int score;

    private void Awake()
    {
        scoreText.text = 0.ToString();
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    public void SetScore(int number)
    {
        score += number;
    }
}
