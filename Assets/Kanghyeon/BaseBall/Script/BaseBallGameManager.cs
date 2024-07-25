using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BaseBallGameManager : WholeGameManager
{

    public static BaseBallGameManager instance;
    public TextMeshProUGUI finishboard;
    public PitcherSquid pitcher;

    public Bat bat;

    // public float score;
    public float starttime;
    public float balltotal;
    public float ballcount;
    public float finalscore;

    public bool IsGameStart = false;
    public bool IsGameEnd = false;
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        isDescend = true;
    }

    public override void GameStart()
    {
        starttime = Time.time;
        pitcher.ShootBall();
        bat.IsGameStart();
        NetworkManager.instance.isDescending = isDescend;
    }

    // public override void GetScore()
    // {
    //     score = finalscore;
    //     AddScore(PhotonNetwork.LocalPlayer.NickName,score);
    // }

    // public override void GameEnd()
    // {
    // }

    public void CalculSocre()
    {
        
    }
    public void CountBall()
    {
        ballcount = ballcount + 1f;
        if (ballcount == balltotal)
        {
            IsGameEnd = true;
            finishboard.text = "End" + " Score:" + score;
            StartCoroutine(EndScene());

        }
    }

    private IEnumerator EndScene()
    {
        yield return new WaitForSeconds(1f);
        TotalManager.instance.StartFinish();
    }

    public void CountScore()
    {
        score = score + 1f;
        CountBall();
    }
    // private void AddScore(string name, float score)
    // {
    //     photonView.RPC("rpcAddScore",RpcTarget.All,name,score);
    // }
    // [PunRPC]
    // void rpcAddScore(string name, float score)
    // {
    //     NetworkManager.instance.currentplayerscore[name] = score;
    // }
  
  
    
}

