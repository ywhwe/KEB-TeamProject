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
    
    public Image textScreen;
    public Image scoreSreen;
    public TextMeshProUGUI textScreenText;
    public Image[] OXImage;
    public Sprite[] OXImageDB;
    private int ballCount;

    public bool hitFlag;

    private void Awake()
    {
        instance = this;
        ballCount = 0;
    }

    public IEnumerator TextScreenOn()
    {
        textScreen.gameObject.SetActive(true);
        if (hitFlag)
        {
            textScreenText.text = "HOME RUN";
            OXImageOn(0);
        }
        else
        {
            textScreenText.text = "STRIKE";
            OXImageOn(1);
        }
        yield return new WaitForSeconds(1f);
        textScreen.gameObject.SetActive(false);
        ballCount++;
        textScreenText.text = "";
    }

    private void OXImageOn(int i)
    {
        OXImage[ballCount].sprite = OXImageDB[i];
        var tempColor = OXImage[ballCount].color;
        tempColor.a = 1f;
        OXImage[ballCount].color = tempColor;
    }
    
}
