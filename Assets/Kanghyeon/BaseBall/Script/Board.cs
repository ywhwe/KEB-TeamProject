using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    public static Board instance;
    
   
    public Image scoreSreen;
    public Image[] OXImage;
    public Sprite[] OXImageDB;
    private int ballCount;

    private void Awake()
    {
        instance = this;
        ballCount = 0;
    }
    

    public void OXImageOn(int i)
    {
        OXImage[ballCount].sprite = OXImageDB[i];
        var tempColor = OXImage[ballCount].color;
        tempColor.a = 1f;
        OXImage[ballCount].color = tempColor;
        ballCount++;
    }
    
}
