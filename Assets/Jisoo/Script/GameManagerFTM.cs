using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class GameManagerFTM : WholeGameManager
{
    public static GameManagerFTM instance;
    
    public float startTime;
    public float finishTime;
    public PhotonView PV;
    
    private void Awake()
    {
        instance = this;
    }

    public override void GameStart()
    {
        RandomMotion.instance.StartGame();
    }

    public override void GetScore()
    {
        score = finishTime - startTime;
        AddScore(PhotonNetwork.LocalPlayer.NickName,score);
    }

    public override void GameEnd()
    {
        TotalManager.instance.StartFinish();
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
