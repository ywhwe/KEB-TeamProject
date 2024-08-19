using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using EPOOutline;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    public GameObject[] playerposdb;

    [SerializeField] 
    private TextMeshProUGUI playernum;
    private GameObject playerpref;
    private GameObject playerpos;

    private CancellationTokenSource cancel;

    private GameObject playerobj;
    public GameObject timer;
    private bool isTimerOn;

    public int ReadyToMatch=0;
    
    [Tooltip("The prefab to use for representing the player")]
    private void Awake()
    {
        Instance = this;
        playerpref = TotalManager.instance.obplayerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(index);
        playerpos= playerposdb[index];
    }

    private void Start()
    {
        TotalManager.instance.gameRound = 0;
        playernum.text = PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        StartCoroutine(DelayInst());
    }

    IEnumerator DelayInst()
    {
        yield return new WaitForSeconds(1f);
        playerobj = PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.identity,0);
        playerobj.GetComponent<Outlinable>().enabled = true;
    }

    #region Photon CallBacks

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Main");
        Debug.Log("Im out");
    }

    private async UniTaskVoid StartCounting()
    {
        for (int i = 5; i < 0; i--)
        {
            await UniTask.WaitForSeconds(1f, cancellationToken:cancel.Token);
        }
        PhotonNetwork.CurrentRoom.IsOpen = false;
        NetworkManager.instance.SendNextGameNum();
        Debug.LogFormat("PhotonNetwork : Loading Level : Round1");
        TotalManager.instance.GoToGameScene();
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        playernum.text = PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        playerpos= playerposdb[index];
        playerobj.transform.position = playerpos.transform.position;
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        if (PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            // timer.SetActive(true);
            // isTimerOn = true;
            if (PhotonNetwork.IsMasterClient)
            {
                // cancel = new CancellationTokenSource();
                // StartCounting().Forget();
            }
            
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
                PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        playerpos= playerposdb[index];
        playerobj.transform.position = playerpos.transform.position;
        playernum.text = PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        if (isTimerOn==true)
        {
            timer.SetActive(false);
        }
        if (PhotonNetwork.IsMasterClient)
        {
            if (cancel != null)
            {
                cancel.Cancel();
                cancel.Dispose();
                cancel = null;
            }
            
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}",
                PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom

        }
   
    }

    #endregion

    #region Public Methods

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        Destroy(GameObject.Find("NetworkManager"));

    }
    
    public void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.LogFormat("PhotonNetwork : Match Start");
        StartMatch().Forget();
    }

    #endregion

    private async UniTaskVoid StartMatch()
    {
        NetworkManager.instance.SendNextGameNum();
        await UniTask.WaitUntil(() => ReadyToMatch == PhotonNetwork.PlayerList.Length);
        await UniTask.WaitForSeconds(1f);
        TotalManager.instance.GoToGameScene();
    }

    
    

}
