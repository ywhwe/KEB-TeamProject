using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using ColorUtility = UnityEngine.ColorUtility;


public class ScoreBoardManager : MonoBehaviourPunCallbacks //점수 계산을 위해 모든 게임의 점수를 int로 순위는 내림차순으로 정리
{
    public TextMeshProUGUI[] scoretxt;
    public TextMeshProUGUI[] ranktxt;
    public TextMeshProUGUI[] nametxt;
    public GameObject[] scorelist;
    public TextMeshProUGUI nexttimer;
    public TextMeshProUGUI winner;
    public PhotonView PV;
    public List<KeyValuePair<string, float>> ranklist;
    public static ScoreBoardManager instance;
    public int isLoadScore=0;
    public GameObject controlpanel;
    public GameObject player;
    public GameObject timer;
    public GameObject Next;
    public RectTransform viewtransform;
    public GameObject[] effect;
    
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
        TotalManager.instance.BGM.Play();
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
            NetworkManager.instance.SelectLoser();
            UpdateScore();
            if (ranklist[0].Key == PhotonNetwork.LocalPlayer.NickName)
            {
                foreach (var VARIABLE in effect)
                {
                    VARIABLE.SetActive(true);
                }
            }
            winner.enabled = true;
            winner.text = "Winner is " + ranklist[0].Key;
            nexttimer.enabled = true;
            nexttimer.text = "Go to Menu";
            controlpanel.SetActive(true);
            return;
        }          
        CalculScore(NetworkManager.instance.currentplayerscore,NetworkManager.instance.isDescending);
        NetworkManager.instance.SelectLoser();
        UpdateScore();
        NetworkManager.instance.SendLoadScore();
    }

    public async UniTaskVoid LoadingTimer() // Need fix " warning CS1998: This async method lacks 'await' operators and will run synchronously.
                                        // Consider using the 'await' operator to await non-blocking API calls,
                                        // or 'await Task.Run(...)' to do CPU-bound work on a background thread."
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
        LoadTimer().Forget(); // warning CS4014: Because this call is not awaited,
                     // execution of the current method continues before the call is completed.
                     // Consider applying the 'await' operator to the result of the call.
    }
    
    private async UniTaskVoid LoadTimer()
    {
        await UniTask.WaitForSeconds(3f);

        if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToMasterServer)
        {
            nexttimer.enabled = true;
            nexttimer.text = "You Failed";
            controlpanel.SetActive(true);
            KickTimer().Forget();
            return;
        }
       
        Next.SetActive(true);
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
    private async UniTaskVoid KickTimer()
    {
        for (int i = 0; i < 11 ; i++)
        {
            await UniTask.WaitForSeconds(1f);
        }
        GoToMainMenu();
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
        Color color;
        int index = 0;
        OutlineMyScore();
        Debug.Log(NetworkManager.instance.loserdb.Count);
        foreach (var player in ranklist)
        {
            if (index < scoretxt.Length)
            {
                var size = viewtransform.sizeDelta;
                size.y += 135;
                viewtransform.sizeDelta = size;
                scorelist[index].SetActive(true);
                nametxt[index].text = player.Key;
                scoretxt[index].text = player.Value.ToString("F2");
                index++;
                ranktxt[index-1].text = index.ToString();
                if (index >= ranklist.Count-NetworkManager.instance.loserdb.Count)
                {
                    if (NetworkManager.instance.loserdb.Count == 0)
                    {
                        continue;
                    }
                    ColorUtility.TryParseHtmlString("#656568", out color);
                    scorelist[index].GetComponent<Image>().color = color;


                }
            }
        }
    }

    private void OutlineMyScore()
    {
        int index = ranklist.FindIndex(x => x.Key == PhotonNetwork.LocalPlayer.NickName);
        scorelist[index].GetComponent<Outline>().enabled = true;
    }
    #endregion


}
