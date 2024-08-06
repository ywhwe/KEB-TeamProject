using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using EPOOutline;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

public class RecordGameManager : WholeGameManager
{
    public static RecordGameManager instance;
    public float recordtime;
    private int recordnum;
    public PhotonView PV;
    public GameObject[] playerposdb; // 각 플레이어 pos 데이터
    private GameObject playerpref; // local 플레이어의 프리펩
    private GameObject playerpos; // local 플레이어의 pos위치
    public NoteSpawner notespawner;
    public RecordNoteData notedb;
    public GameObject record;

    private float speed = 0f;
    private float angle = 0f;
    private bool isGamestart = false;
    #region Audio

    public AudioSource audio;
    public AudioClip[] audiodb;
    
    private void MusicStart(int num)
    {
        audio.clip = audiodb[num];
        if (num==2)
        {
            audio.PlayDelayed(0.3f);
            return;
        }

        audio.PlayDelayed(0.25f);

    }
    

    #endregion
    
    #region ShuffleGame
    public List<int> gameindex;
    private int gameready=0;
    private int gameround = 0;


    private void Shuffle(int num)
    {
        if (num == 1)
        {
            gameindex = new List<int>() { 0, 1, 2 };
        }
        if (num == 2)
        {
            gameindex = new List<int>() { 0, 2, 1 };
        }
        if (num == 3)
        {
            gameindex = new List<int>() { 1, 0, 2 };
        }
        if (num == 4)
        {
            gameindex = new List<int>() { 1, 2, 0 };
        }
        if (num == 5)
        {
            gameindex = new List<int>() { 2, 0, 1 };
        }
        if (num == 6)
        {
            gameindex = new List<int>() { 2, 1, 0 };
        }
    }
    
    public void Sendindex()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int id = Random.Range(1, 7);
            PV.RPC("rpcSendindex",RpcTarget.All,id);
        }
    }
    
    [PunRPC]
    void rpcSendindex(int id)
    {
        Shuffle(id);
    }

    #region CheckReady

    public void CheckReady()
    {
        PV.RPC("rpcCheckReady",RpcTarget.MasterClient);
    }

    [PunRPC]
    void rpcCheckReady()
    {
        gameready++;
        if (gameready == PhotonNetwork.PlayerList.Length)
        {
            Sendindex();
        }
    }

    #endregion
    #endregion
    private void Awake()
    {
        instance = this;
        recordnum = 0;
        playerpref = TotalManager.instance.obplayerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(index);
        playerpos= playerposdb[index];
        
    }
    private void Start()
    {
        NetworkManager.instance.isDescending = false;
        CheckReady();
    }

    private void Update()
    {
        if (isGamestart == true)
        {
            angle = angle + Time.deltaTime * speed;
            record.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            if (angle >= 360)
            {
                angle = 0f;
                MusicStart(gameindex[gameround]);
            }
        }
    }

    private void RotateRecord()
    {
        
    }

    // IEnumerator DelayInst() //플레이어 instant 함수
    // {
    //     yield return new WaitForSeconds(1f);
    //     PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.Euler(0f,90f,0f));
    // }
    public override void GameStart()
    {
        recordtime = Time.time;
        StartCycle(gameindex[0]);
    }

    private void StartCycle(int index)
    {
        speed = notedb.notepos[index].rotatespeed;
        MusicStart(index);
        isGamestart = true;
       

    }

    public override void SpawnObsPlayer()
    {
        var localojb = PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.Euler(0f,90f,0f),0);
        localojb.GetComponent<Outlinable>().enabled = true;
        // localojb.GetComponent<PhotonTransformView>().m_SynchronizePosition = false;

    }
    public override void ReadyForStart()
    {
        angle = 0f;
        record.transform.rotation = Quaternion.Euler(0f, angle, 0f);
        notespawner.GetComponent<NoteSpawner>().RecordingNote(gameindex[gameround]);
    }

    // public override void GetScore()
    // {
    //     score = recordtime;
    //     AddScore(PhotonNetwork.LocalPlayer.NickName,score);
    //     
    // }

    // public override void GameEnd()
    // {
    //    
    //     TotalManager.instance.StartFinish();
    // }

    public void PlusCountRecord()
    {
        recordnum++;
    }

    public async UniTask CountScoreRecord()
    {
        recordnum--;
        if (recordnum == 0)
        {
            audio.Stop();
            angle = 0f;
            isGamestart = false;
            gameround++;
            if (gameround == 3)
            {
                score = Time.time - recordtime;
                TotalManager.instance.StartFinish();
            }
            else
            {
                await TotalManager.instance.UniReadyCount();
                StartCycle(gameindex[gameround]);
            }
          
            Debug.Log(score);
            
        }
    }
    
    // private void AddScore(string name, float score)
    // {
    //     PV.RPC("rpcAddScore",RpcTarget.All,name,score);
    // }
    // [PunRPC]
    // void rpcAddScore(string name, float score)
    // {
    //     NetworkManager.instance.currentplayerscore[name] = score;
    // }

}
