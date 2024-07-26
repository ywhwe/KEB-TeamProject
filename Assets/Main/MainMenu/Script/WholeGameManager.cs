using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public abstract class WholeGameManager : MonoBehaviourPunCallbacks
{
    public float score; // Should be protected
    public bool isGameEnd;
    public bool isDescend;
    
    
    
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
