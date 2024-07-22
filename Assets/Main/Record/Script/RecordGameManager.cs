using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecordGameManager : WholeGameManager
{
    public static RecordGameManager instance;
    public float rotatespeed;
    public RecordRotate record;
    public float recordtime;
    public override void GameStart()
    {
        record.enabled = true;
        recordtime = Time.time;
    }

    public override void GetScore()
    {
        
    }

    public override void GameEnd()
    {
        recordtime = Time.time - recordtime;
    }
    

    void Update()
    {
        record.transform.Rotate(Vector3.down * (rotatespeed * Time.deltaTime));
    }

}
