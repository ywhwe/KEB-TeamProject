using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class score : MonoBehaviour
{
    public samplemove sample;
    public TextMeshProUGUI scoreboard;
    private float moverange;

    private float checktime;
    // Start is called before the first frame update
    void Start()
    {
        checktime = 0f;
        moverange = 10f / sample.movespeed;
    }

    private void Update()
    {
        checktime += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            CheckScore(checktime);
        }
    }

    // Update is called once per frame
    public void CheckScore(float time)
    {
        
        
        if (0<=(moverange-time)&&(moverange-time)<0.3f)
        {
            scoreboard.text = "perfect";
        }
        else if (0<=(moverange-time)&&(moverange-time)<0.6f)
        {
            scoreboard.text = "good";
        }
        else
        {
            scoreboard.text = "miss";
        }
    }
    
    
}
