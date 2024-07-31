using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviourPunCallbacks //점수 계산을 위해 모든 게임의 점수를 int로 순위는 내림차순으로 정리
{
    public TextMeshProUGUI[] scoretxt;
    public TextMeshProUGUI nexttimer;
    public TextMeshProUGUI winner;
    public PhotonView PV;
    public List<KeyValuePair<string, float>> ranklist;
    public static ScoreBoardManager instance;
    public int isLoadScore=0;
    public GameObject controlpanel;
    public GameObject player;
    
    public void NextGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        TotalManager.instance.GoToGameScene();
    }

    public void GoToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("NetworkManager"));
        TotalManager.instance.MoveScene("Main");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Im out2");
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        GameObject playerPrefab = Instantiate(TotalManager.instance.obplayerPrefab, player.transform.position, player.transform.rotation);
        playerPrefab.transform.SetParent(player.transform);
        playerPrefab.transform.localScale = new Vector3(300f, 300f, 300f);
    }

    private void Start()
    {
        if (TotalManager.instance.gameRound==3)
        {
            CalculScore(NetworkManager.instance.currentplayerscore,NetworkManager.instance.isDescending);
            UpdateScoreUI();
            winner.text = "Winner is " + ranklist[0].Key;
            nexttimer.text = "Go to Menu";
            controlpanel.SetActive(true);
            return;
        }
        CalculScore(NetworkManager.instance.currentplayerscore,NetworkManager.instance.isDescending);
        UpdateScoreUI();
        NetworkManager.instance.SendLoadScore();
    }

    public async UniTask LoadingTimer()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        // await UniTask.WaitUntil(() => isLoadScore == PhotonNetwork.PlayerList.Length);
        PV.RPC("rpcRunTimer",RpcTarget.All);
    }

    [PunRPC]
    void rpcRunTimer()
    {
        LoadTimer();
    }
    
    private async UniTask LoadTimer()
    {
        await UniTask.WaitForSeconds(3f);

        if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            nexttimer.text = "You Failed";
            controlpanel.SetActive(true);
            return;
        }
       
        int time = 5;
        for (int i = 0; i < 6; i++)
        {
            nexttimer.text = time.ToString();
            if (time==0)
            {
                NextGame();
            }
            time--;
            await UniTask.WaitForSeconds(1f);
        }
    }

    #region Score
    
    public void CalculScore(GenericDictionary<string,float> scoredb,bool Descending)
    {
        // var sortedscore = scoredb.OrderByDescending(x => x.Value).ToList();
        //
        // int currentRankPt = 4;
        // float currentScore = sortedscore[0].Value;
        // int k = 0;
        // for (int i = 0; i < sortedscore.Count; i++)  //점수계산을 위한 식
        // {
        //     if (sortedscore[i].Value < currentScore) // 점수가 감소되면 차등된 점수를 받음, 동점일때를 위한 점수 계산을 위해 k를 사용
        //     {
        //         currentRankPt -= k;
        //         k = 0;
        //         k++;
        //         currentScore = sortedscore[i].Value;
        //         finalscore[sortedscore[i].Key] += currentRankPt;
        //         continue;
        //     }
        //
        //     k++;
        //     finalscore[sortedscore[i].Key] += currentRankPt; // 점수변화가 없으면 계속 같은점수를 받음
        // }
        if (Descending)
        {
            ranklist = scoredb.OrderByDescending(x => x.Value).ToList();
        }
        else
        {
            ranklist = scoredb.OrderBy(x => x.Value).ToList();
        }
    }

    public void UpdateScoreUI()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            photonView.RPC("UpdateScore", RpcTarget.All);
        }
    }

    [PunRPC]
    public void UpdateScore()
    {
        int index = 0;
        foreach (var player in ranklist)
        {
            if (index < scoretxt.Length)
            {
                scoretxt[index].text = player.Key + ":" + player.Value;
                index++;
            }
        }
    }
    #endregion


}
