using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using TMPro;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviourPunCallbacks //점수 계산을 위해 모든 게임의 점수를 int로 순위는 내림차순으로 정리
{
    public TextMeshProUGUI[] scoretxt;
    public PhotonView PV;
    private GenericDictionary<string, int> finalscore;
    public void RestartGame()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }
        TotalManager.instance.GoToGameWith();
    }

    public void GoToMainMenu()
    {
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("NetworkManager"));
        TotalManager.instance.MoveScene(0);
    }

    
    private void Start()
    {
        finalscore = NetworkManager.instance.playerscores;
        CalculScore(NetworkManager.instance.currentplayerscore);
        UpdateScoreUI();
        NetworkManager.instance.playerscores = finalscore;
        NetworkManager.instance.InitCurScore();
    }

    public void CalculScore(GenericDictionary<string,int> scoredb)
    {
        var sortedscore = scoredb.OrderByDescending(x => x.Value).ToList();

        int currentRankPt = 4;
        int currentScore = sortedscore[0].Value;
        int k = 0;
        for (int i = 0; i < sortedscore.Count; i++)  //점수계산을 위한 식
        {
            if (sortedscore[i].Value < currentScore) // 점수가 감소되면 차등된 점수를 받음, 동점일때를 위한 점수 계산을 위해 k를 사용
            {
                currentRankPt -= k;
                k = 0;
                k++;
                currentScore = sortedscore[i].Value;
                finalscore[sortedscore[i].Key] += currentRankPt;
                continue;
            }

            k++;
            finalscore[sortedscore[i].Key] += currentRankPt; // 점수변화가 없으면 계속 같은점수를 받음
        }

        finalscore.OrderByDescending(x => x.Value);
    }

    public void UpdateScoreUI()
    {
        photonView.RPC("UpdateScore",RpcTarget.All);
    }

    [PunRPC]
    public void UpdateScore()
    {
        int index = 0;
        foreach (var player in finalscore)
        {
            if (index < scoretxt.Length)
            {
                scoretxt[index].text = player.Key + ":" + player.Value;
                index++;
            }
        }
    }

   
}
