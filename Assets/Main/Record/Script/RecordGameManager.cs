using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RecordGameManager : WholeGameManager
{
    public static RecordGameManager instance;
    public float rotatespeed;
    public RecordRotate record;
    public float recordtime;
    private int recordnum;
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        recordnum = 0;
    }

    public override void GameStart()
    {
        record.enabled = true;
        recordtime = Time.time;
    }

    public override void GetScore()
    {
        score = 10000 - recordtime * 10;
        AddScore(PhotonNetwork.LocalPlayer.NickName,score);
        
    }

    public override void GameEnd()
    {
        recordtime = Time.time - recordtime;
        TotalManager.instance.StartFinish();
    }

    public void PlusCountRecord()
    {
        recordnum++;
    }

    public void CountScoreRecord()
    {
        recordnum--;
        if (recordnum == 0)
        {
            GameEnd();
        }
    }
    
    private void AddScore(string name, float score)
    {
        PV.RPC("rpcAddScore",RpcTarget.All,name,score);
    }
    [PunRPC]
    void rpcAddScore(string name, float score)
    {
        NetworkManager.instance.currentplayerscore[name] = score;
    }

}
