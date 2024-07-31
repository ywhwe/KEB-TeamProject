using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Photon.Pun;


public class MemoryGameManager : WholeGameManager
{
    public static MemoryGameManager instance;

    public GameObject[] playerPosDB;
    private GameObject playerPref;
    private GameObject playerPos;
    
    public float limitTime;
    public TurnInfo[] turnDB;
    private CharacterMotionController playerController;
    public Animator cpuAni;
    
    private List<int> randomMotions = new();
    private int playerInputIdx = 0;
    private int turn = 0;
    private float getScore = 0f;
    
    private bool isPlayerTurn;
    private bool isTimedOut;
    private bool isTimerActive;

    public PhotonView PV;
    
    private Coroutine cpuMotionPlayCoroutine;
    
    private float playingTime = 0;
  
    private int[] motionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isAMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isDMove")
    };

    public GameObject[] indexDirection;
    
    protected static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
    
    private void Awake()
    {
        instance = this;
        playerPref = TotalManager.instance.memoryGamePrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        playerPos = playerPosDB[index];
        Debug.Log(index);
        
        NetworkManager.instance.isDescending = true; // If change score system make this false
        StartCoroutine(DelayInst());
    }
    
    IEnumerator DelayInst() //플레이어 instant 함수
    {
        yield return new WaitForSeconds(1f);
        var playerObj = PhotonNetwork.Instantiate("MemoryGamePrefab/" + playerPref.name, playerPos.transform.position, playerPos.transform.rotation);
        playerObj.transform.localScale = playerPos.transform.localScale;
        playerController = playerObj.GetComponent<CharacterMotionController>();
        playerController.OnKeyPressed += PlayerInput;
    }
    
    private void Update()
    { 
        Timer();
    }
    
    public override void GameStart()
    { 
        StartGame();
    }

    public override void SpawnObsPlayer()
    {
        
    }

    private void StartGame()
    {
        isTimerActive = true;
       cpuMotionPlayCoroutine = StartCoroutine(PlayRandomMotion());
    }
    
    public void SelectRandomMotion()
    {
        randomMotions.Clear();
        for (int i = 0; i < turnDB[turn].numOfSeq; i++)
        {
            randomMotions.Add(Random.Range(0, 4));
        }
    }
    
    private IEnumerator PlayMotion()
    {
        for (int i = 0; i < randomMotions.Count; i++)
        {
            Debug.Log(randomMotions[i]);
            cpuAni.SetTrigger(motionHash[randomMotions[i]]);
            indexDirection[randomMotions[i]].SetActive(true);
            cpuAni.SetFloat(MotionSpeed, 1f/turnDB[turn].motionPlayTime);
          ;

            yield return new WaitForSeconds(turnDB[turn].motionPlayTime);
            indexDirection[randomMotions[i]].SetActive(false);
        }
    }
    
    private IEnumerator PlayRandomMotion()
    {
        Debug.Log($"Current turn is {turn}");
        isPlayerTurn = false;
        playerController.SetActiveInput(false);
        
        SelectRandomMotion();
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(PlayMotion());

        playerInputIdx = 0;
        playerController.SetActiveInput(true);
        isPlayerTurn = true;
    }
    
    private void PlayerInput(int motionIdx)
    {
        if (!isPlayerTurn) return;

       /* if (motionIdx == 1) motionIdx = 3;
        if (motionIdx == 3) motionIdx = 1;*/
        
        if (randomMotions[playerInputIdx] == motionIdx)
        {
            //나아중에 이펙트 재생 (후순위)
            Debug.Log("Correct");
        }
        else
        {
            Debug.Log("Incorrect");
            cpuMotionPlayCoroutine = StartCoroutine(PlayRandomMotion());
        }
        
        playerInputIdx++;

        if (playerInputIdx == randomMotions.Count)
        {
            turn++;

            if (turn >= turnDB.Length)
            {
                isTimerActive = false;
                FinishGame();
                
            }
            else
            {
                StartCoroutine(PlayRandomMotion());
               
            }
        }
    }

    private void FinishGame()
    {
        //게임하는 시간 기록, 제한시간 내에 못 끝내면 turn수가 점수...
        //시간이 끝났을 때 강제 종료
        
        if(cpuMotionPlayCoroutine != null)
            StopCoroutine(cpuMotionPlayCoroutine);
        
        playerController.SetActiveInput(false);
        
        if (isTimedOut)       
        {
            getScore = turn;
        }
        else 
        {
            getScore = limitTime - playingTime + turnDB.Length;
        }
        
        Debug.Log($"Score is {getScore}");
        
        score = getScore;
        TotalManager.instance.StartFinish();
    }

    private void Timer()
    {
        if (!isTimerActive) return;
        playingTime += Time.deltaTime;

        if (playingTime > limitTime)
        {
            Debug.Log("TIMES UP!");
            isTimedOut = true;
            FinishGame(); // 게임 종료
            isTimerActive = false;
        }
    }
}

[Serializable]
public class TurnInfo
{
    public int numOfSeq;
    public float motionPlayTime;
}