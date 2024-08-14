using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    [Tooltip("The Ui Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;
    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 12;

    private string gameversion = "1";

    private bool isConnecting;

    private TransitionManager manager;

    public TransitionSettings transition;
    // Start is called before the first frame update
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    public void Connect()
    {
        manager = TransitionManager.Instance();
        manager.onTransitionCutPointReached += ConnectTran;
        manager.Transition(transition,0.01f);
    }

    public void ConnectTran()
    {
        progressLabel.SetActive(true);
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = gameversion;
        }
        manager.onTransitionCutPointReached -= ConnectTran;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Launcher: OnConnectedToMaster() was called by PUN");
        if (isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
    }
    public void OnDisconnected(DisconnectCause cause)
    {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

        PhotonNetwork.CreateRoom(null, new RoomOptions{MaxPlayers = maxPlayersPerRoom});
    }
    public override void OnJoinedRoom()
    {
        progressLabel.SetActive(false);
        Debug.Log("Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            Debug.Log("We load the 'Lobby' ");

            // #Critical
            // Load the Room Level.
            PhotonNetwork.LoadLevel("Lobby2");
        }
    }
    
}
