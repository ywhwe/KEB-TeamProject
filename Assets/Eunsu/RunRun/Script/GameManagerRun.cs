using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Photon.Pun;

public class GameManagerRun : WholeGameManager // Need fix for inheritance
{
    public static GameManagerRun instance;
    public PhotonView pvTest;
    
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Background.bgInstance.BackgroundMove().Forget();
        StartCoroutine(NoteController.instance.GenNotes());
        // TwoKeyPlayer.playerInstance.KeyInteraction().Forget();
    }
    // private void Start()
    // {
    //     Background.bgInstance.BackgroundMove().Forget();
    //     StartCoroutine(NoteController.instance.GenNotes()); // This will be deleted when build version
    // }

    private void Update()
    {
        if (!(Time.deltaTime > 1f)) return;
        Destroy(NoteController.instance.gameObject);
    }
    
    private void LateUpdate()
    {
        if (NoteController.instance.noteCount < 1 && NoteController.instance.IsTimedOut)
            NoteController.instance.IsFinished = true;
        
        if (NoteController.instance.IsFinished)
        {
            StopCoroutine(NoteController.instance.GenNotes());
            UniTask.WaitForSeconds(1f);
            GameEnd();
        }
    }
    
    void rpcAddScore(string curName, float curScore)
    {
        NetworkManager.instance.currentplayerscore[curName] = curScore;
    }

    public override void GameStart()
    {
        Background.bgInstance.BackgroundMove().Forget();
        StartCoroutine(NoteController.instance.GenNotes());
        TwoKeyPlayer.playerInstance.KeyInteraction().Forget();
    }

    public override void GetScore()
    {
        score = ScoreBoard.scoreInstance.score;
        pvTest.RPC("rpcAddScore",RpcTarget.All,PhotonNetwork.LocalPlayer.NickName,score);
    }

    public override void GameEnd()
    {
        TotalManager.instance.StartFinish();
    }
}
