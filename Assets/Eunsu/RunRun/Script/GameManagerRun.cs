using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Photon.Pun;

public class GameManagerRun : WholeGameManager // Need fix for inheritance
{
    public static GameManagerRun instance;
    private float playerScore;
    public PhotonView pvTest;
    private bool isGameEnded = false;
    public GameObject[] playerposdb; // 각 플레이어 pos 데이터
    private GameObject playerpref; // local 플레이어의 프리펩
    private GameObject playerpos; // local 플레이어의 pos위치
    private void Awake()
    {
        instance = this;
        playerpref = TotalManager.instance.obplayerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        Debug.Log(index);
        playerpos= playerposdb[index];

    }

    private void Start()
    {
        NetworkManager.instance.isDescending = true;
    }

    private void Update()
    {
        if (!(Time.deltaTime > 1f)) return;
        Destroy(NoteController.instance.gameObject);
    }
    
    private void LateUpdate()
    {
        if (NoteController.instance.noteCount < 1 && NoteController.instance.IsTimedOut)
            NoteController.instance.IsFinished = true;
        
        if (!NoteController.instance.IsFinished) return;

        if (isGameEnded) return;
        StopCoroutine(NoteController.instance.GenNotes());
        StartCoroutine(EndScene());
    }

    public override void GameStart()
    {
        CharacterMotionController.instance.isTwoKey = true;
        Background.bgInstance.BackgroundMove().Forget();
        StartCoroutine(NoteController.instance.GenNotes());
    }

    public override void SpawnObsPlayer()
    {
        var obj = PhotonNetwork.Instantiate(playerpref.name, playerpos.transform.position, Quaternion.identity);
        obj.transform.SetParent(playerpos.transform);
        obj.transform.localScale = Vector3.one;
    }
    private IEnumerator EndScene()
    {
        score = ScoreBoard.scoreInstance.score;
        isGameEnded = true;
        CharacterMotionController.instance.isTwoKey = false;
        yield return new WaitForSeconds(1f);
        TotalManager.instance.StartFinish();
    }
}
