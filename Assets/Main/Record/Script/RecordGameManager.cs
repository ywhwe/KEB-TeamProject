using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class RecordGameManager : WholeGameManager
{
    public static RecordGameManager instance;
    public float rotatespeed;
    public RecordRotate record;
    public float recordtime;
    private int recordnum;
    public PhotonView PV;
    public GameObject[] playerposdb; // 각 플레이어 pos 데이터
    private GameObject playerpref; // local 플레이어의 프리펩
    private GameObject playerpos; // local 플레이어의 pos위치

    private void Awake()
    {
        instance = this;
        recordnum = 0;
        playerpref = TotalManager.instance.obplayerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(index);
        playerpos= playerposdb[index];
        
    }
    private void Start()
    {
        NetworkManager.instance.isDescending = false;
    }
    // IEnumerator DelayInst() //플레이어 instant 함수
    // {
    //     yield return new WaitForSeconds(1f);
    //     PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.Euler(0f,90f,0f));
    // }
    public override void GameStart()
    {
        record.enabled = true;
        recordtime = Time.time;
    }

    public override void SpawnObsPlayer()
    {
        PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.Euler(0f,90f,0f),0);
    }

    // public override void GetScore()
    // {
    //     score = recordtime;
    //     AddScore(PhotonNetwork.LocalPlayer.NickName,score);
    //     
    // }

    // public override void GameEnd()
    // {
    //    
    //     TotalManager.instance.StartFinish();
    // }

    public void PlusCountRecord()
    {
        recordnum++;
    }

    public void CountScoreRecord()
    {
        recordnum--;
        if (recordnum == 0)
        {
            score = Time.time - recordtime;
            Debug.Log(score);
            TotalManager.instance.StartFinish();
        }
    }
    
    // private void AddScore(string name, float score)
    // {
    //     PV.RPC("rpcAddScore",RpcTarget.All,name,score);
    // }
    // [PunRPC]
    // void rpcAddScore(string name, float score)
    // {
    //     NetworkManager.instance.currentplayerscore[name] = score;
    // }

}
