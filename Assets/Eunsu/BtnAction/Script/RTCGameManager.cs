using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using Photon.Pun;

public class RTCGameManager : WholeGameManager
{
    public static RTCGameManager instance;

    private const float StartTime = 120.00f;
    
    public const int NumberOfButtons = 50;
    
    [SerializeField] private AudioSource audioSource;
    
    [Header("Trolley")]
    public GameObject trolleyPrefab;
    private readonly Vector3 trolleyPos = new (2f, 0.44f, -0.197f);
    
    [Header("Buttons")]
    [SerializeField] private GameObject W;
    [SerializeField] private GameObject A;
    [SerializeField] private GameObject S;
    [SerializeField] private GameObject D;
    private GameObject waitingKey;

    [Header("Counter")]
    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI successCounter;
    
    [HideInInspector] public bool flag = true; // This will be false when game is ended
    
    [HideInInspector] public KeyCode waitingKeyCode = KeyCode.None;
    
    [HideInInspector] public int successCount, btnNumber;

    [HideInInspector] public bool isLegal = true; // This checks if user input is consecutive but mostly incorrect

    [HideInInspector] public bool isGen; // This will be true if button has generated
    
    private float rand, clearTime, delayNext;
    
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        
        TotalManager.instance.SendMessageSceneStarted();
        
        successCount = 0;
        clearTime = 0;
        btnNumber = 0;
        delayNext = 1.2f;
        
        successCounter.text = successCount.ToString();
        timeCounter.text = StartTime.ToString("F2");
    }

    private void Start()
    {
        NetworkManager.instance.isDescending = false;
        RTCObjMover.RtcObjInstance.BgMove().Forget();
        RTCObjMover.RtcObjInstance.RailMove().Forget();
        isGameEnd = false;
        
        audioSource.Play();
    }

    private void Update()
    {
        if (clearTime > 120f) isGameEnd = true;
    }
    
    // Whether user input is correct or not, buttons are keep generated
    private async UniTask GenBtn()
    {
        while (!isGameEnd)
        {
            GenControl();
            btnNumber++;
            
            // Controls timing of button spawn
            delayNext = btnNumber switch
            {
                15 => 0.9f,
                35 => 0.7f,
                _ => delayNext
            };
            
            await UniTask.WaitForSeconds(delayNext);
            
            waitingKey.SetActive(false);
            
            if (successCount is NumberOfButtons) break;
        }

        isGameEnd = true;
    }
    
    private void GenControl()
    {
        float random = Random.Range(0, 100);
        
        switch (random)
        {
            case >= 0 and < 25 :
                W.SetActive(true);
                waitingKey = W;
                waitingKeyCode = KeyCode.W;
                isGen = true;
                break;
            case >= 25 and < 50 :
                A.SetActive(true);
                waitingKey = A;
                waitingKeyCode = KeyCode.A;
                isGen = true;
                break;
            case >= 50 and <75 :
                S.SetActive(true);
                waitingKey = S;
                waitingKeyCode = KeyCode.S;
                isGen = true;
                break;
            case >= 75 and < 100 :
                D.SetActive(true);
                waitingKey = D;
                waitingKeyCode = KeyCode.D;
                isGen = true;
                break;
        }
    }

    private async UniTask InputControl()
    {
        while (!isGameEnd)
        {
            if (isLegal)
            {
                RTCButtonController.ctrlInstance.SetKey();
            }

            if (RTCButtonController.ctrlInstance.inputKeyCode is KeyCode.None)
            {
                await UniTask.Yield();
                continue;
            }

            await CompKey();
        }
        
        StartCoroutine(EndScene());
    }
    
    private async UniTask CompKey()
    {
        if (waitingKeyCode == RTCButtonController.ctrlInstance.inputKeyCode && isLegal)
        {
            successCount++;
            successCounter.text = successCount.ToString();
            
            await AllowInput();

            RTCObjMover.RtcObjInstance.AccelerationSpeed().Forget();
            SoundManagerForRTC.instance.PlaySound("Accel");
            
            await UniTask.WaitForSeconds(0.5f);
        }
        else
        {
            SoundManagerForRTC.instance.PlaySound("Break");
            
            await DenyInput();
        }
        
        RTCButtonController.ctrlInstance.inputKeyCode = KeyCode.None;
    }

    private async UniTask AllowInput()
    {
        isLegal = true;
        
        await UniTask.Yield();
    }

    private async UniTask DenyInput()
    {
        isLegal = false;
        
        RTCObjMover.RtcObjInstance.DecelerationSpeed().Forget();
        await UniTask.WaitForSeconds(2f);
        
        isLegal = true;
    }
    
    private async UniTask TimeCount()
    {
        while(!isGameEnd)
        {
            clearTime += Time.deltaTime;
            timeCounter.text = (StartTime - clearTime).ToString("F2");

            await UniTask.Yield();
        }
    }
    
    public override void GameStart()
    {
        Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        
        RTCObjMover.RtcObjInstance.Spin().Forget();
        
        TimeCount().Forget();
        InputControl().Forget();
        GenBtn().Forget();
    }

    public override void SpawnObsPlayer()
    {
        
    }
    public override void ReadyForStart()
    {
        
    }
    
    private IEnumerator EndScene()
    {
        if (!isGameEnd) yield break;
        score = clearTime;
        
        yield return new WaitForSeconds(1f);
        flag = false;
        
        audioSource.Stop();
        
        TotalManager.instance.StartFinish();
    }
}

[Serializable]
public struct Buttons
{
    public enum Position
    {
        Left,
        Mid,
        Right
    }

    [SerializeField] private Position position;
    
    [SerializeField] public GameObject W;
    [SerializeField] public GameObject A;
    [SerializeField] public GameObject S;
    [SerializeField] public GameObject D;
}