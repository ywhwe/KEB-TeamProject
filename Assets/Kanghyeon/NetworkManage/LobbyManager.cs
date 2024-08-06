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

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public static LobbyManager Instance;
    public GameObject[] playerposdb;

    [SerializeField] 
    private TextMeshProUGUI playernum;
    private GameObject playerpref;
    private GameObject playerpos;

    private CancellationTokenSource cancel;
    
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
        var localojb = PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.identity,0);
        localojb.GetComponent<Outlinable>().enabled = true;
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
        Debug.LogFormat("PhotonNetwork : Loading Level : baseball");
        TotalManager.instance.GoToGameScene();
    }
    
    public override void OnPlayerEnteredRoom(Photon.Realtime.Player other)
    {
        playernum.text = PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName); // not seen if you're the player connecting
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.PlayerList.Length == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                cancel = new CancellationTokenSource();
                StartCounting().Forget();
            }
            
            //
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}",
                PhotonNetwork.IsMasterClient); // called before OnPlayerLeftRoom
        }
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player other)
    {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName); // seen when other disconnects
        playernum.text = PhotonNetwork.PlayerList.Length + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
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

    #endregion

    #region Private Methods

    public void LoadArena()
    {
        if (!PhotonNetwork.IsMasterClient)
        {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            return;
        }

        PhotonNetwork.CurrentRoom.IsOpen = false;
        Debug.LogFormat("PhotonNetwork : Loading Level : baseball");
        TotalManager.instance.GoToGameScene();
    }

    #endregion

}
