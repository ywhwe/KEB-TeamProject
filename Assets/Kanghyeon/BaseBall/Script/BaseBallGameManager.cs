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
    public int balltotal;
    public int ballcount;
    public int finalscore;

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

    public void CountBall()
    {
        ballcount++;
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
        finalscore++;
        CountBall();
    }
    public void AddScore(string name, int score)
    {
        PV.RPC("rpcAddScore",RpcTarget.All,name,score);
    }
    [PunRPC]
    void rpcAddScore(string name, int score)
    {
        NetworkManager.instance.currentplayerscore[name] = score;
    }
  
  
    
}

