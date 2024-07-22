using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
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
    }

    public override void GameStart()
    {
        starttime = Time.time;
        pitcher.ShootBall();
        bat.IsGameStart();
    }

    public override void GetScore()
    {
        score = finalscore;
    }

    public override void GameEnd()
    {
        AddScore(PhotonNetwork.LocalPlayer.NickName,finalscore);
        TotalManager.instance.ScoreBoardTest();
    }

    public void CalculSocre()
    {
        
    }
    public void CountBall()
    {
        ballcount = ballcount + 1f;
        if (ballcount == balltotal)
        {
            IsGameEnd = true;
            finishboard.text = "End" + " Score:" + finalscore;
            StartCoroutine(EndScene());

        }
    }

    private IEnumerator EndScene()
    {
        yield return new WaitForSeconds(1f);
        GameEnd();
    }

    public void CountScore()
    {
        finalscore = finalscore + 1f;
        CountBall();
    }
    public void AddScore(string name, float score)
    {
        PV.RPC("rpcAddScore",RpcTarget.All,name,score);
    }
    [PunRPC]
    void rpcAddScore(string name, float score)
    {
        NetworkManager.instance.currentplayerscore[name] = score;
    }
  
  
    
}

