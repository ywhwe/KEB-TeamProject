using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class BaseBallGameManager : WholeGameManager
{

    public static BaseBallGameManager instance;
    public TextMeshProUGUI finishboard;
    public PitcherSquid pitcher;

    public Bat bat;


    // public float score;
    public float balltotal;
    public float ballcount;
    private int roundcount=0;
    public float finalscore;

    public bool IsGameStart = false;
    public bool IsGameEnd = false;
    public PhotonView PV;

    public AudioSource audio;
    #region ShuffleGame
    public List<int> gameindex;
    private int gameready=0;

    private void Shuffle(int num)
    {
        if (num == 1)
        {
            gameindex = new List<int>() { 0, 1, 2 };
        }
        if (num == 2)
        {
            gameindex = new List<int>() { 0, 2, 1 };
        }
        if (num == 3)
        {
            gameindex = new List<int>() { 1, 0, 2 };
        }
        if (num == 4)
        {
            gameindex = new List<int>() { 1, 2, 0 };
        }
        if (num == 5)
        {
            gameindex = new List<int>() { 2, 0, 1 };
        }
        if (num == 6)
        {
            gameindex = new List<int>() { 2, 1, 0 };
        }
    }
    
    public void Sendindex()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int id = Random.Range(1, 7);
            PV.RPC("rpcSendindex",RpcTarget.All,id);
        }
    }
    
    [PunRPC]
    void rpcSendindex(int id)
    {
        Shuffle(id);
    }

    #region CheckReady

    public void CheckReady()
    {
        PV.RPC("rpcCheckReady",RpcTarget.MasterClient);
    }

    [PunRPC]
    void rpcCheckReady()
    {
        gameready++;
        if (gameready == PhotonNetwork.PlayerList.Length)
        {
            Sendindex();
        }
    }

    #endregion
    #endregion
    private void Awake()
    {
        instance = this;
        isDescend = true;
        TotalManager.instance.SendMessageSceneStarted();
    }

    private void Start()
    {
        
        audio.Play();
        CheckReady();
    }

    public override void GameStart()
    {
        pitcher.StartCycle(gameindex[0]);
        bat.IsGameStart();
        NetworkManager.instance.isDescending = isDescend;
    }

    public override void SpawnObsPlayer()
    {
        
    }
    public override void ReadyForStart()
    {
        
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
    public async UniTask CountBall()
    {
        ballcount = ballcount + 1f;
        if (ballcount == 9)
        {
            roundcount++;
            ballcount = 0;
            if (roundcount==3)
            {
                IsGameEnd = true;
                finishboard.text = "End" + " Score:" + score;
                audio.Stop();
                StartCoroutine(EndScene());
            }
            else
            {
                await TotalManager.instance.UniReadyCount();
                pitcher.StartCycle(gameindex[roundcount]);
            }
            
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

