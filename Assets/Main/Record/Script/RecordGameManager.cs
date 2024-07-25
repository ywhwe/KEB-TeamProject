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
        StartCoroutine(DelayInst());
    }
    IEnumerator DelayInst() //플레이어 instant 함수
    {
        yield return new WaitForSeconds(1f);
        PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.Euler(0f,90f,0f));
    }

    public override void GameStart()
    {
        record.enabled = true;
        recordtime = Time.time;
    }

    public override void GetScore()
    {
        score = 10000 - recordtime * 10;
        AddScore(PhotonNetwork.LocalPlayer.NickName,score);
        
    }

    public override void GameEnd()
    {
        recordtime = Time.time - recordtime;
        TotalManager.instance.StartFinish();
    }

    public void PlusCountRecord()
    {
        recordnum++;
    }

    public void CountScoreRecord()
    {
        recordnum--;
        if (recordnum == 0)
        {
            GameEnd();
        }
    }
    
    private void AddScore(string name, float score)
    {
        PV.RPC("rpcAddScore",RpcTarget.All,name,score);
    }
    [PunRPC]
    void rpcAddScore(string name, float score)
    {
        NetworkManager.instance.currentplayerscore[name] = score;
    }

}
