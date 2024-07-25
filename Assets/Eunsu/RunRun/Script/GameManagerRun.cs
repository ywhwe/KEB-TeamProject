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
    
    private void Awake()
    {
        instance = this;
    }

    /*private void Start()
    {
        Background.bgInstance.BackgroundMove().Forget();
        StartCoroutine(NoteController.instance.GenNotes());
        // TwoKeyPlayer.playerInstance.KeyInteraction().Forget();
    }*/

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
    
    [PunRPC]
    private void RPCAddScore(string curName, float curScore)
    {
        NetworkManager.instance.currentplayerscore[curName] = curScore;
    }

    public override void GameStart()
    {
        Background.bgInstance.BackgroundMove().Forget();
        StartCoroutine(NoteController.instance.GenNotes());
    }

    public override void GetScore()
    {
        score = ScoreBoard.scoreInstance.score;
        pvTest.RPC("RPCAddScore",RpcTarget.All,PhotonNetwork.LocalPlayer.NickName,score);
    }

    public override void GameEnd()
    {
        TotalManager.instance.StartFinish();
    }
    
    private IEnumerator EndScene()
    {
        isGameEnded = true;
        yield return new WaitForSeconds(1f);
        GameEnd();
    }
}
