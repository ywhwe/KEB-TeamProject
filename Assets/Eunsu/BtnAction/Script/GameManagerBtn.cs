using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using Photon.Pun;

public class GameManagerBtn : WholeGameManager
{
    public static GameManagerBtn instance;

    [SerializeField] private AudioSource audioSource;

    [HideInInspector]
    public bool flag = true; // This will be false when game is ended
    
    [Header("Trolley")]
    public GameObject trolleyPrefab;
    private readonly Vector3 trolleyPos = new (2f, 0.44f, -0.197f);
    
    [ArrayElementTitle("position")]
    [Header("Buttons")]
    [SerializeField] private Buttons[] genParts;
    
    [Header("Buttons")]
    [SerializeField] private GameObject W;
    [SerializeField] private GameObject A;
    [SerializeField] private GameObject S;
    [SerializeField] private GameObject D;
    private GameObject[] waitingKey;

    [Header("Counter")]
    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI successCounter;
    
    [HideInInspector]
    public KeyCode waitingKeyCode = KeyCode.None;
    
    [HideInInspector]
    public int successCount;

    private float rand, clearTime;
    
    private const float StartTime = 120.00f;
    
    public const int NumberOfButtons = 50;
    
    private bool isMatch = true; // This checks user input is correct

    [HideInInspector] public bool isLegal = true; // This checks if user input is consecutive but mostly incorrect

    [HideInInspector] public bool isGen; // This will be true if button has generated

    private bool isAccel;

    private int level;
    
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        TotalManager.instance.SendMessageSceneStarted();
        successCount = 0;
        clearTime = 0;
        level = 0;
        
        successCounter.text = successCount.ToString();
        timeCounter.text = StartTime.ToString("F2");
    }

    private void Start()
    {
        NetworkManager.instance.isDescending = false;
        ObjMover.ObjInstance.BgMove().Forget();
        ObjMover.ObjInstance.RailMove().Forget();
        isGameEnd = false;
        isAccel = false;
        
        audioSource.Play();
    }

    private void Update()
    {
        if (clearTime > 120f) isGameEnd = true;
    }
    
    private async UniTask GenQTE()
    {
        while (!isGameEnd)
        {
            await UniTask.WaitUntil(() => !isAccel);
            
            BtnControl();
            
            // if IsMatch is false, suspends coroutine 'til it is true
            await UniTask.WaitUntil(() => isMatch);

            successCount++;

            level = successCount switch
            {
                >= 15 and < 35 => 1,
                >= 35 => 2,
                _ => level
            };
            
            successCounter.text = successCount.ToString();
            
            waitingKey[0].SetActive(false);
            waitingKey[1].SetActive(false);
            waitingKey[2].SetActive(false);
            
            if (successCount is NumberOfButtons) break;
        }

        isGameEnd = true;
    }
    
    private void BtnControl()
    {
        switch (level)
        {
            case 0:
                genLogic();
                break;
            case 1:
                genLogic(level);
                break;
            case 2:
                genLogic(level, true);
                break;
        }
    }

    private async UniTask InputControl()
    {
        while (!isGameEnd)
        {
            isMatch = false;

            if (isLegal)
            {
                BtnController.ctrlInstance.SetKey();
            }

            if (BtnController.ctrlInstance.inputKeyCode is KeyCode.None)
            {
                await UniTask.Yield();
                continue;
            }

            await CompKey();
            isAccel = false;
        }
        
        StartCoroutine(EndScene());
    }
    
    private async UniTask CompKey()
    {
        if (waitingKeyCode == BtnController.ctrlInstance.inputKeyCode && isLegal)
        {
            isAccel = true;
            await AllowInput();

            ObjMover.ObjInstance.AccelerationSpeed().Forget();
            SoundManagerForBtnAction.instance.PlaySound("Accel");
            
            await UniTask.WaitForSeconds(0.5f);
        }
        else
        {
            SoundManagerForBtnAction.instance.PlaySound("Break");
            
            await DenyInput();
        }
        
        BtnController.ctrlInstance.inputKeyCode = KeyCode.None;
    }

    private async UniTask AllowInput()
    {
        isMatch = true;
        isLegal = true;
        
        await UniTask.Yield();
    }

    private async UniTask DenyInput()
    {
        isMatch = false;
        isLegal = false;
        
        ObjMover.ObjInstance.DecelerationSpeed().Forget();
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

    private void genLogic()
    {
        float random = Random.Range(0, 100);
        
        switch (random)
        {
            case >= 0 and < 25 :
                genParts[1].W.SetActive(true);
                waitingKey[1] = W;
                waitingKeyCode = KeyCode.W;
                isGen = true;
                break;
            case >= 25 and < 50 :
                genParts[1].A.SetActive(true);
                waitingKey[1] = A;
                waitingKeyCode = KeyCode.A;
                isGen = true;
                break;
            case >= 50 and <75 :
                genParts[1].S.SetActive(true);
                waitingKey[1] = S;
                waitingKeyCode = KeyCode.S;
                isGen = true;
                break;
            case >= 75 and < 100 :
                genParts[1].D.SetActive(true);
                waitingKey[1] = D;
                waitingKeyCode = KeyCode.D;
                isGen = true;
                break;
        }
    }
    
    private void genLogic(int hmm)
    {
        for (var i = 1; i < 3; i++)
        {
            var j = 2 * i - 2;
            
            float random = Random.Range(0, 100);

            switch (random)
            {
                case >= 0 and < 25 :
                    genParts[j].W.SetActive(true);
                    waitingKey[j] = W;
                    waitingKeyCode = KeyCode.W;
                    isGen = true;
                    break;
                case >= 25 and < 50 :
                    genParts[j].A.SetActive(true);
                    waitingKey[j] = A;
                    waitingKeyCode = KeyCode.A;
                    isGen = true;
                    break;
                case >= 50 and <75 :
                    genParts[j].S.SetActive(true);
                    waitingKey[j] = S;
                    waitingKeyCode = KeyCode.S;
                    isGen = true;
                    break;
                case >= 75 and < 100 :
                    genParts[j].D.SetActive(true);
                    waitingKey[j] = D;
                    waitingKeyCode = KeyCode.D;
                    isGen = true;
                    break;
            }
        }
    }
    
    private void genLogic(int hmm, bool hmmm)
    {
        for (var i = 0; i < 2; i++)
        {
            float random = Random.Range(0, 100);

            switch (random)
            {
                case >= 0 and < 25 :
                    genParts[i].W.SetActive(true);
                    waitingKey[i] = W;
                    waitingKeyCode = KeyCode.W;
                    isGen = true;
                    break;
                case >= 25 and < 50 :
                    genParts[i].A.SetActive(true);
                    waitingKey[i] = A;
                    waitingKeyCode = KeyCode.A;
                    isGen = true;
                    break;
                case >= 50 and <75 :
                    genParts[i].S.SetActive(true);
                    waitingKey[i] = S;
                    waitingKeyCode = KeyCode.S;
                    isGen = true;
                    break;
                case >= 75 and < 100 :
                    genParts[i].D.SetActive(true);
                    waitingKey[i] = D;
                    waitingKeyCode = KeyCode.D;
                    isGen = true;
                    break;
            }
        }
    }
    
    public override void GameStart()
    {
        Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        
        ObjMover.ObjInstance.Spin().Forget();
        
        TimeCount().Forget();
        InputControl().Forget();
        GenQTE().Forget();
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