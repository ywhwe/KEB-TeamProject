using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public abstract class WholeGameManager : MonoBehaviourPunCallbacks
{
    protected float score;
    public bool isGameEnd;
    public bool isDescend;
    
    
    
    public abstract void GameStart();

    public  void GetScore()
    {
        photonView.RPC("rpcAddScore",RpcTarget.All,name,score);
    }

    // public abstract void GameEnd();
    

    [PunRPC]
    void rpcAddScore(string name, float score)
    {
        NetworkManager.instance.currentplayerscore[name] = score;
    }

}
