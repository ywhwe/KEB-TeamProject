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

    public bool hitFlag;

    private void Awake()
    {
        instance = this;
    }

    public IEnumerator TextScreenOn()
    {
        if (hitFlag)
        {
            textScreenText.text = "HOME RUN";
        }
        else
        {
            textScreenText.text = "STRIKE";
        }
        textScreen.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        textScreen.gameObject.SetActive(false);
        textScreenText.text = "";
        
    }
}
