using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public List<string> scoredb;
    public GenericDictionary<string, float> currentplayerscore = new GenericDictionary<string, float>();
    
    // public Dictionary<int, int> rankPt = new Dictionary<int, int>()
    // {
    //     {4,10},{3,5},{2,3},{1,1}
    // };

    public bool isDescending;
    public int isLoadScene = 0;
    
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
        InitCurScore();
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
        InitCurScore();
    }
    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("Lefted Player initScore");
        currentplayerscore.Remove(other.NickName);
    }

    public void SendLoadScore()
    {
        photonView.RPC("rpcSendLoadScore",RpcTarget.MasterClient);
    } 

    [PunRPC]
    void rpcSendLoadScore()
    {
        IsLoadScore();
    }
    public async UniTask IsLoadScore()
    {
        isLoadScene++;
        if (isLoadScene==PhotonNetwork.PlayerList.Length)
        {
            ScoreBoardManager.instance.LoadingTimer();
            SendKickRoom();
            
            isLoadScene = 0;
        }
    }
    async UniTask SendKickRoom()
    {
        string name = ScoreBoardManager.instance.ranklist[^1].Key;
        Debug.Log(name);
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == name);
        Debug.Log(index);
        
        photonView.RPC("rpcKickRoom",PhotonNetwork.PlayerList[index]);
    } 

    [PunRPC]
    void rpcKickRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

}
