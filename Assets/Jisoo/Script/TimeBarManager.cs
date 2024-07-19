using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeBarManager : MonoBehaviour
{
    public static TimeBarManager instance;
    
    public RectTransform timeBar;
    private float timeBarSize = 500.0f;
    private float waitForStart;
    
    private void Awake()
    {
        instance = this;
        waitForStart = 0f;
    }

    private void Update()
    {
        if (waitForStart <= 5f)
        {
            waitForStart += Time.deltaTime;
        }
        else
        {
            var tempSize = timeBar.sizeDelta;
            tempSize.x += 100f * Time.smoothDeltaTime;
            timeBar.sizeDelta = tempSize;

            if (tempSize.x >= 500f)
            {
                tempSize.x = 0f;
                timeBar.sizeDelta = tempSize;
            }
        }
    }

}
