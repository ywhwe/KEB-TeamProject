using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public abstract class WholeGameManager : MonoBehaviourPunCallbacks
{
    protected float score;
    protected bool isGameEnd;
    protected bool isDescend;
    
    
    
    public abstract void GameStart();

    public abstract void SpawnObsPlayer();
    public  void GetScore()
    {
        photonView.RPC("rpcAddScore",RpcTarget.All,PhotonNetwork.LocalPlayer.NickName,score);
        Debug.Log(score);
    }

    // public abstract void GameEnd();
    

    [PunRPC]
    protected void rpcAddScore(string name, float score)
    {
        Debug.Log(score);
        NetworkManager.instance.currentplayerscore[name] = score;
    }

}
