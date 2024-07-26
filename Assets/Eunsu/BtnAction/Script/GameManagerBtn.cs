using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;
using Photon.Pun;
using UnityEngine.Serialization;

public class GameManagerBtn : WholeGameManager
{
    public static GameManagerBtn instance;

    [HideInInspector]
    public bool flag = true; // This will be false when game is ended
    
    [Header("Trolley")]
    public GameObject trolleyPrefab;
    private Vector3 trolleyPos = new (2f, 0.25f, -0.197f);
    
    [Header("Button")]
    public TextMeshProUGUI qteBtnText;
    
    [HideInInspector]
    public KeyCode waitingKeyCode = KeyCode.None;
    
    private float reductionRate = 2f; // This is for calculating score
    private float rand;
    
    [HideInInspector]
    public int successCount;

    private float clearTime;
    
    private bool isMatch = true; // This checks user input is correct
    public bool IsMatch => isMatch;
    
    [HideInInspector]
    public bool isLegal = true; // This checks if user input is consecutive but mostly incorrect

    [HideInInspector]
    public bool isGen = false; // This will be true if button has generated
    
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        successCount = 0;
    }

    private void Start()
    {
        NetworkManager.instance.isDescending = false;
        ObjMover.ObjInstance.BgMove().Forget();
        ObjMover.ObjInstance.RailMove().Forget();
        isGameEnd = false;
    }

    private async void Update()
    {
        isMatch = false;
        
        clearTime += Time.deltaTime;

        if (isLegal)
        {
            BtnController.ctrlInstance.SetKey();
        }

        if (BtnController.ctrlInstance.inputKeyCode is KeyCode.None) return;

        if (isLegal)
        {
            await CompKey();
        }
        
        if (!isGameEnd) return;
        StartCoroutine(EndScene());
        isGameEnd = false; // Load scoreboard scene just for one time

    }
    
    private async UniTask GenQTE()
    {
        while (!isGameEnd)
        {
            rand = Random.Range(0, 100);
            BtnControl(rand);
            
            // if IsMatch is false, suspends coroutine 'til it is true
            await UniTask.WaitUntil(() => IsMatch);

            if (!IsMatch) continue; // user input matched with QTE buttons, increase successCount
            successCount++;

            if (successCount is 10) break;
        }

        isGameEnd = true;
        Debug.Log("All Cleared in " + clearTime + " sec");
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
    
    private async UniTask CompKey()
    {
        if (waitingKeyCode == BtnController.ctrlInstance.inputKeyCode && isLegal)
        {
            await AllowInput();
            
            ObjMover.ObjInstance.SpeedController().Forget();
        }
        else
        {
            await DenyInput();
        }
        
        BtnController.ctrlInstance.inputKeyCode = KeyCode.None;
    }

    private async UniTask AllowInput()
    {
        isMatch = true;
        
        await UniTask.Yield();
    }

    private async UniTask DenyInput()
    {
        isMatch = false;
        isLegal = false;
        
        await UniTask.WaitForSeconds(2f);
        isLegal = true;
    }
    
    public override void GameStart()
    {
        Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        ObjMover.ObjInstance.Spin().Forget();
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
        TotalManager.instance.StartFinish();
    }
}
