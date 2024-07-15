using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CountingText : MonoBehaviour
{
    public static CountingText instance;
    
    public TextMeshProUGUI countText;
    private WaitForSeconds aSecond = new WaitForSeconds(1f);
    private int count = 5;

    private void Awake()
    {
        instance = this;
    }

    public void CountiongScreenOn()
    {
        this.GameObject().SetActive(true);
        StartCoroutine(Counting());
        this.GameObject().SetActive(false);
    }
    
    public IEnumerator Counting()
    {
        if (count == 0)
        {
            countText.text = "START!";
            yield return aSecond;
            count = 5;
        }
        else
        {
            countText.text = count.ToString();
            count--;
            yield return aSecond;
            StartCoroutine(Counting());
        }
    }
}
