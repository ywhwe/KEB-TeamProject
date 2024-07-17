using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public List<string> scoredb;
    public GenericDictionary<string, int> playerscores = new GenericDictionary<string, int>();
    public GenericDictionary<string, int> currentplayerscore = new GenericDictionary<string, int>();
    
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        scoredb = new List<string>();
        Debug.Log("netM on");
        InitScore();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitScore()
    {
        playerscores.Clear();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            playerscores[player.NickName] = 0;
        }
  
    }

    public void InitCurScore()
    {
        currentplayerscore.Clear();
        foreach (var player in PhotonNetwork.PlayerList)
        {
            currentplayerscore[player.NickName] = 0;
        }
    }
    

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("New Player initScore");
        Debug.Log(playerscores);
        InitScore();
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("Lefted Player initScore");
        playerscores.Remove(other.NickName);
        currentplayerscore.Remove(other.NickName);
    }
    
    //모든 게임에서 쓰일 점수를 등록하는 함수

}
