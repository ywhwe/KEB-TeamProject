using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
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
    private List<string> loserdb = new List<string>();
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
            
            SelectLoser();
            ScoreBoardManager.instance.LoadingTimer();
            SendKickRoom();
            
            isLoadScene = 0;
        }
    }
    async UniTask SendKickRoom()
    {
        
        List<int> indexlist = new List<int>();
        foreach (var VAR in loserdb)
        {
            Debug.Log(VAR);
            int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == VAR);
            Debug.Log(index);
            indexlist.Add(index);
        }
        loserdb = new List<string>();
        foreach (var VAR in indexlist)
        {
            photonView.RPC("rpcKickRoom",PhotonNetwork.PlayerList[VAR]);
        }
    }

    public void SelectLoser()
    {
        var ranklist = ScoreBoardManager.instance.ranklist;
        int length = ranklist.Count - 1;
        float score = ranklist[length].Value;
        int count = 0;
        int target = 1;
        if (!isDescending)
        {
            while (target >= 0)
            {
                if (length - 1 < 0)
                {
                    break;
                }
                if (score == ranklist[length - 1].Value)
                {
                    loserdb.Add(ranklist[length].Key);
                    length--;
                    count++;
                    continue;
                }
                target = target - count;
                count = 0;
                if (score > ranklist[length - 1].Value)
                {
                    if (target <= 0 )
                    {
                        break;
                    }
                    loserdb.Add(ranklist[length].Key);
                    target--;
                    length--;
                }
            }
        }
  
        while (target >= 0)
        {
            if (length - 1 < 0)
            {
                break;
            }
            if (score == ranklist[length - 1].Value)
            {
                loserdb.Add(ranklist[length].Key);
                length--;
                count++;
                continue;
            }
            target = target - count;
            count = 0;
            if (score < ranklist[length - 1].Value)
            {
                if (target <= 0 )
                {
                    break;
                }
                loserdb.Add(ranklist[length].Key);
                target--;
                length--;
            }
        }
    }

    [PunRPC]
    void rpcKickRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

}
