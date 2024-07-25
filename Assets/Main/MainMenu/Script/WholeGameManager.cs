using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;


public abstract class WholeGameManager : MonoBehaviourPunCallbacks
{
    public float score;
    public bool isGameEnd;
    public bool isDescend;
    
    
    
    public abstract void GameStart();

    public abstract void GetScore();
    // {
    //     AddScore(PhotonNetwork.LocalPlayer.NickName,score);
    // }

    public abstract void GameEnd();
    
    // private void AddScore(string name, float score)
    // {
    //     photon.RPC("rpcAddScore",RpcTarget.All,name,score);
    // }
    // [PunRPC]
    // void rpcAddScore(string name, float score)
    // {
    //     NetworkManager.instance.currentplayerscore[name] = score;
    // }

}
