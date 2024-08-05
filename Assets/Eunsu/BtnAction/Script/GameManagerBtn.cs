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
    private GameObject trolleyClone;
    private readonly Vector3 trolleyPos = new (2f, 0.44f, -0.197f);
    
    [Header("Button")]
    public TextMeshProUGUI qteBtnText;

    [Header("Counter")]
    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI successCounter;
    
    [HideInInspector]
    public KeyCode waitingKeyCode = KeyCode.None;
    
    [HideInInspector]
    public int successCount;

    private float rand;

    private float clearTime;
    
    private const float StartTime = 120.00f;
    
    public const int NumberOfButtons = 50;
    
    private bool isMatch = true; // This checks user input is correct

    [HideInInspector] public bool isLegal = true; // This checks if user input is consecutive but mostly incorrect

    [HideInInspector] public bool isGen; // This will be true if button has generated

    private bool isAccel;
    
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        
        successCount = 0;
        clearTime = 0;
        
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
        
        audioSource.PlayOneShot(audioSource.clip);
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
            
            rand = Random.Range(0, 100);
            BtnControl(rand);
            
            // if IsMatch is false, suspends coroutine 'til it is true
            await UniTask.WaitUntil(() => isMatch);

            successCount++;
            successCounter.text = successCount.ToString();
            
            if (successCount is NumberOfButtons) break;
        }

        isGameEnd = true;
    }
    
    private void BtnControl(float random)
    {
        switch (random)
        {
            case >= 0 and < 25 :
                qteBtnText.text = "[ W ]";
                waitingKeyCode = KeyCode.W;
                isGen = true;
                break;
            case >= 25 and < 50 :
                qteBtnText.text = "[ A ]";
                waitingKeyCode = KeyCode.A;
                isGen = true;
                break;
            case >= 50 and <75 :
                qteBtnText.text = "[ S ]";
                waitingKeyCode = KeyCode.S;
                isGen = true;
                break;
            case >= 75 and < 100 :
                qteBtnText.text = "[ D ]";
                waitingKeyCode = KeyCode.D;
                isGen = true;
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
    
    public override void GameStart()
    {
        trolleyClone = Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        
        ObjMover.ObjInstance.Spin().Forget();
        
        TimeCount().Forget();
        InputControl().Forget();
        GenQTE().Forget();
    }

    public override void SpawnObsPlayer()
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
