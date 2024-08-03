using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
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
    
    public GameObject[] indexDirection;
    public GameObject[] playerIndexDirection;
    public GameObject[] stamp;
    
    public Transform timer;
    public Vector3 maxTimeBar;
    public Vector3 emptyTimeBar;
    public SpriteRenderer timerColor;
    public Color white;
    public Color red;
    
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
    private Coroutine[] playerIndexCor = new Coroutine[4];

    public GameObject[] round1;
    public GameObject[] round10;
    public GameObject startRound;
    
    private float playingTime = 0;
    
  
    private int[] motionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isAMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isDMove")
    };
    
    protected static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");
    
    private void Awake()
    {
        instance = this;
        playerPref = TotalManager.instance.playerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        playerPos = playerPosDB[index];
        Debug.Log(index);
        
        NetworkManager.instance.isDescending = true; // If change score system make this false
        StartCoroutine(DelayInst());
        startRound.SetActive(true);
    }
    
    IEnumerator DelayInst() //플레이어 instant 함수
    {
        yield return new WaitForSeconds(1f);
        var playerObj = PhotonNetwork.Instantiate("MainAnimal/FREE/Prefabs/Player Prefab/"+ playerPref.name, playerPos.transform.position, playerPos.transform.rotation);
        playerObj.transform.localScale = playerPos.transform.localScale;
        playerController = playerObj.GetComponent<CharacterMotionController>();
        playerController.isMirrored = true;
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
          

            yield return new WaitForSeconds(turnDB[turn].motionPlayTime);
            indexDirection[randomMotions[i]].SetActive(false);
        }
    }
    
    private IEnumerator PlayRandomMotion()
    {
        ;

        Debug.Log($"Current turn is {turn}");
        isPlayerTurn = false;
        playerController.SetActiveInput(false);
        
    
        SelectRandomMotion();
        yield return new WaitForSeconds(1f);
        startRound.SetActive(false);
             round1[turn % 10].SetActive(false);
             round1[(turn % 10)+1].SetActive(true);
                 
             if (turn >= 10)
             {
                 round10[turn%100].SetActive(false);
                 round10[(turn%100)+1].SetActive(true);
             }
                
             else
             {
                 round10[0].SetActive(true);
             }
                
        stamp[0].SetActive(false);
        stamp[1].SetActive(false);
        
        yield return new WaitForSeconds(0.5f); 
        
        yield return StartCoroutine(PlayMotion());

        playerInputIdx = 0;
        playerController.SetActiveInput(true);
        isPlayerTurn = true;
    }

    IEnumerator falsePlayerIndex(int motionIdx)
    {
        playerIndexDirection[motionIdx].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        playerIndexDirection[motionIdx].SetActive(false);
    }
    private void PlayerInput(int motionIdx)
    {
        if (!isPlayerTurn) return;
        if (playerIndexCor[motionIdx] != null)
        { 
            StopCoroutine(playerIndexCor[motionIdx]);
        }
    
        playerIndexCor[motionIdx] = StartCoroutine(falsePlayerIndex(motionIdx));
       
        
        
        if (randomMotions[playerInputIdx] == motionIdx)
        {
            //나아중에 이펙트 재생 (후순위)
            stamp[0].SetActive(true);
            Debug.Log("Correct");
        }
        else
        {
            stamp[0].SetActive(false);
            stamp[1].SetActive(true);
            Debug.Log("Incorrect"); 
            cpuMotionPlayCoroutine = StartCoroutine(PlayRandomMotion());
            return;
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
                cpuMotionPlayCoroutine = StartCoroutine(PlayRandomMotion());
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
        timer.position = Vector3.Lerp(maxTimeBar, emptyTimeBar, playingTime / limitTime);
        timerColor.color = Color.Lerp(white,red,playingTime / limitTime);
        
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