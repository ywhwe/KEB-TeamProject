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
    public bool flag = true;
    
    [Header("Trolley")]
    public GameObject trolleyPrefab;
    private Vector3 trolleyPos = new (2f, 0.25f, -0.197f);
    [HideInInspector]
    public GameObject trolleyClone;
    
    [Header("Button")]
    public TextMeshProUGUI qteBtnText;
    [HideInInspector]
    public KeyCode waitingKeyCode = KeyCode.None;
    
    private float reductionRate = 2f;
    private float rand;
    [HideInInspector]
    public int successCount;
    
    private float objSpeed = 1f;
    
    private bool isMatch = true;
    public bool IsMatch => isMatch;

    [HideInInspector]
    public bool isGen = false;
    
    public PhotonView PV;

    private void Awake()
    {
        instance = this;
        successCount = 0;
    }

    private void Start()
    {
        ObjMover.ObjInstance.BgMove().Forget();
        ObjMover.ObjInstance.RailMove().Forget();
        score = 1000;
        isGameEnd = false;
    }

    private void Update()
    {
        isMatch = false;
        score -= Time.deltaTime * reductionRate;
        
        BtnController.ctrlInstance.SetKey();
        if (BtnController.ctrlInstance.inputKeyCode is KeyCode.None) return;
        CompKey();

        if (isGameEnd)
        {
            StartCoroutine(EndScene());
            isGameEnd = false;
        }
        
    }
    
    private async UniTask GenQTE()
    {
        while (successCount < 10) // if successCount bigger than setting value stops loop
        {
            rand = Random.Range(0, 100);
            BtnControl(rand);
            
            // if IsMatch is false, suspends coroutine 'til it is true
            await UniTask.WaitUntil(() => IsMatch);

            if (!IsMatch) continue; // user input matched with QTE buttons, increase successCount
            successCount++;
        }

        isGameEnd = true;
        Debug.Log("Remain score: " + score);
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
    
    private async void CompKey()
    {
        if (waitingKeyCode == BtnController.ctrlInstance.inputKeyCode)
        {
            BtnController.ctrlInstance.inputKeyCode = KeyCode.None;
            isMatch = true;
            ObjMover.ObjInstance.angle = 34f;
            ObjMover.ObjInstance.SpeedController().Forget();
            await UniTask.Delay(1000);
            ObjMover.ObjInstance.angle = 3f;
        }
        else
        {
            BtnController.ctrlInstance.inputKeyCode = KeyCode.None;
            await UniTask.WaitForSeconds(1f);
            isMatch = false;
            ObjMover.ObjInstance.angle = 3f;
        }
    }
    
    public override void GameStart()
    {
        trolleyClone = Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        ObjMover.ObjInstance.Spin(trolleyClone).Forget();
        GenQTE().Forget();
    }

    public override void GetScore()
    {
        PV.RPC("RPCAddScore",RpcTarget.All,PhotonNetwork.LocalPlayer.NickName,score);
    }

    public override void GameEnd()
    {
        flag = false;
        TotalManager.instance.StartFinish();
    }
    
    private IEnumerator EndScene()
    {
        if (!isGameEnd) yield break;
        yield return new WaitForSeconds(1f);
        GameEnd();
    }
}
