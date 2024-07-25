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
        NetworkManager.instance.isDescending = true;
    }

    public override void GameStart()
    {
        RandomMotion.instance.StartGame();
    }
}
