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
using Random = UnityEngine.Random;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    public static NetworkManager instance;
    public List<string> scoredb;
    public GenericDictionary<string, float> currentplayerscore = new GenericDictionary<string, float>();

    private int gamenum1;
    private int gamenum2;
    private int gamenum3;
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
        int target = Mathf.CeilToInt(PhotonNetwork.PlayerList.Length / 3f);
        Debug.Log(target);
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
        if (isDescending)
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
                if (score < ranklist[length - 1].Value)
                {
                    if (target <= 0)
                    {
                        break;
                    }

                    loserdb.Add(ranklist[length].Key);
                    target--;
                    length--;
                }
            }
        }
    }

    [PunRPC]
    void rpcKickRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    private void SelectNextGameNum()
    {
        gamenum1 = (Random.Range(1,4));
        gamenum2 = (Random.Range(4,6));
        gamenum3 = (6);
    }

    public void SendNextGameNum()
    {
        SelectNextGameNum();
        photonView.RPC("rpcSendNextGameNum",RpcTarget.All,gamenum1,gamenum2,gamenum3);
    }
    [PunRPC]
    void rpcSendNextGameNum(int num1 , int num2 , int num3)
    {
        photonView.RPC("rpcSendReady",RpcTarget.MasterClient);
        TotalManager.instance.NextgameNum.Add(0);
        TotalManager.instance.NextgameNum.Add(2);  //round1
        TotalManager.instance.NextgameNum.Add(num2);  //round2
        TotalManager.instance.NextgameNum.Add(num3);  //round3
    }

    [PunRPC]
    void rpcSendReady()
    {
        LobbyManager.Instance.ReadyToMatch++;
    }
}
