using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using EPOOutline;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;
using Photon.Pun;


public class MemoryGameManager : WholeGameManager
{
    public static MemoryGameManager instance;

    public GameObject[] playerPosDB;
    public GameObject Mypos;
    private GameObject playerPref;
    private GameObject playerPos;

    public GameObject[] indexDirection;
    public GameObject[] playerIndexDirection;
    public GameObject[] stamp;

    private GameObject playerObj;
    public GameObject cpuObj;

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

    private int prevRound = 0;
    public GameObject[] round1;
    public GameObject[] round10;

    private int prevTime;
    public GameObject[] time1;
    public GameObject[] time10;

    private float playingTime = 0;

    private int[] motionHash =
    {
        Animator.StringToHash("isWMove"),
        Animator.StringToHash("isAMove"),
        Animator.StringToHash("isSMove"),
        Animator.StringToHash("isDMove")
    };

    public ParticleSystem correctEffect;
    public ParticleSystem inCorrectffect;

    protected static readonly int MotionSpeed = Animator.StringToHash("MotionSpeed");

    private void Awake()
    {
        instance = this;
        TotalManager.instance.SendMessageSceneStarted();
        InitNumbers();
        
        playerPref = TotalManager.instance.playerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        playerPos = playerPosDB[index];
        Debug.Log(index);

        NetworkManager.instance.isDescending = true; // If change score system make this false
       
    }

    // IEnumerator DelayInst() //플레이어 instant 함수
    // {
    //     yield return new WaitForSeconds(1f);
    //     
    // }

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
        spawn().Forget();
    }

    private async UniTaskVoid spawn()
    {
        playerPref = TotalManager.instance.playerPrefab;
        int index = Array.FindIndex(PhotonNetwork.PlayerList, x => x.NickName == PhotonNetwork.LocalPlayer.NickName);
        playerPos = playerPosDB[index];
        
        playerObj = PhotonNetwork.Instantiate(playerPref.name, 
            playerPos.transform.position, playerPos.transform.rotation);
        playerObj.transform.localScale = new Vector3(2f, 2f, 2f);
        
        //await UniTask.WaitForSeconds(0.3f);
        
        playerObj.GetComponent<PhotonTransformView>().m_SynchronizePosition = false;
        playerObj.GetComponent<Outlinable>().enabled = true;
        playerObj.transform.position = Mypos.transform.position;
        
        playerController = playerObj.GetComponent<CharacterMotionController>();
        playerController.isMirrored = true;
        playerController.OnKeyPressed += PlayerInput;
        
    }

    public override void ReadyForStart()
    {
       
    }

    private void InitNumbers()
    {
        prevTime = (int)limitTime;
        
        round1[0].SetActive(true);
        round10[0].SetActive(true);
           
        time1[CalDigit(prevTime, 0)].SetActive(true);
        time10[CalDigit(prevTime, 1)].SetActive(true);
    }
    
    private void StartGame()
    {
        isTimerActive = true;
        cpuMotionPlayCoroutine = StartCoroutine(PlayRandomMotion());
    }

    private void Timer()
    {
        if (!isTimerActive) return;
        
        playingTime += Time.deltaTime;
        
        timer.position = Vector3.Lerp(maxTimeBar, emptyTimeBar, playingTime / limitTime);
        timerColor.color = Color.Lerp(white, red, playingTime / limitTime);

        int showTime = (int)(limitTime - playingTime);
        ShowTimeNum(showTime);

        if (playingTime > limitTime)
        {
            Debug.Log("TIMES UP!");
            isTimedOut = true;
            FinishGame(); // 게임 종료
            isTimerActive = false;
        }
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
            cpuAni.SetFloat(MotionSpeed, 1f / turnDB[turn].motionPlayTime);

            StartCoroutine(ShowCPUIndex(randomMotions[i],turnDB[turn].motionPlayTime * 0.9f));
            //인덱스재생
            yield return new WaitForSeconds(turnDB[turn].motionPlayTime);
        }
    }
    
    IEnumerator ShowCPUIndex(int motionIdx, float motionPlayTime)
    {
        indexDirection[motionIdx].SetActive(true);
        yield return new WaitForSeconds(motionPlayTime);
        indexDirection[motionIdx].SetActive(false);
    }

    

    private IEnumerator PlayRandomMotion()
    
    {
        playerObj.GetComponent<Outlinable>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        cpuObj.GetComponent<Outlinable>().enabled = true;
        Debug.Log($"Current turn is {turn}");
        isPlayerTurn = false;
        playerController.SetActiveInput(false);
        
        SelectRandomMotion();
        yield return new WaitForSeconds(0.7f);

        ShowRoundNum(turn + 1);

        stamp[0].SetActive(false);
        stamp[1].SetActive(false);
        
        yield return new WaitForSeconds(0.5f);
        inCorrectffect.Stop();

        yield return StartCoroutine(PlayMotion());

        playerInputIdx = 0;
        playerController.SetActiveInput(true);
        cpuObj.GetComponent<Outlinable>().enabled = false;    
        isPlayerTurn = true;
        playerObj.GetComponent<Outlinable>().enabled = true;
    }

    IEnumerator ShowPlayerIndex(int motionIdx)
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

        playerIndexCor[motionIdx] = StartCoroutine(ShowPlayerIndex(motionIdx));
        
        if (randomMotions[playerInputIdx] == motionIdx)
        {
            correctEffect.Play();
            Debug.Log("Correct");
        }
        else
        {
            stamp[0].SetActive(false);
            stamp[1].SetActive(true);
            Debug.Log("Incorrect");
            inCorrectffect.Play();
            cpuMotionPlayCoroutine = StartCoroutine(PlayRandomMotion());
            return;
        }

        playerInputIdx++;

        if (playerInputIdx == randomMotions.Count)
        {
            stamp[0].SetActive(true);
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

    private void ShowTimeNum(int value)
    {
        if (value == prevTime) return;

        time1[CalDigit(prevTime, 0)].SetActive(false);
        time10[CalDigit(prevTime, 1)].SetActive(false);

        time1[CalDigit(value, 0)].SetActive(true);
        time10[CalDigit(value, 1)].SetActive(true);

        prevTime = value;
    }

    private void ShowRoundNum(int value)
    {
        round1[CalDigit(prevRound, 0)].SetActive(false);
        round10[CalDigit(prevRound, 1)].SetActive(false);

        round1[CalDigit(value, 0)].SetActive(true);
        round10[CalDigit(value, 1)].SetActive(true);

        prevRound = value;
    }

    private int CalDigit(int value, int digit)
    {
        var digitNum = (int)Math.Pow(10, digit); // 1234 / digit = 2 --> 100
        value /= digitNum; // 12
        return (value % 10);
    }
    
    private void FinishGame()
    {
        //게임하는 시간 기록, 제한시간 내에 못 끝내면 turn수가 점수...
        //시간이 끝났을 때 강제 종료

        if (cpuMotionPlayCoroutine != null)
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
    
}

[Serializable]
public class TurnInfo
{
    public int numOfSeq;
    public float motionPlayTime;
}