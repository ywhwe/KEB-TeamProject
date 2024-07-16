using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManagerBtn : WholeGameManager
{
    public static GameManagerBtn instance;

    [HideInInspector]
    public bool flag = true;
    
    public GameObject railPrefab;
    public GameObject trolleyPrefab;

    private GameObject rails;
    private GameObject rails1;
    private Transform railTrans;
    private Vector3 railMoveVector = new (-3f, 0f, 0f);
    
    private Vector3 railPos = new (-1.84f, 0.05f, 1.04f);
    private Vector3 nextRailPos = new(10f, 0.05f, 1.04f);
    private Vector3 trolleyPos = new (2f, 0.25f, -0.197f);
    
    private float railSpeed = 1f;
    private float timer = 0f;

    private void Awake()
    {
        // When GameManager inheritance Fixed, this should be in StartGame()
        instance = this; 
    }

    private void Start()
    {
        BGMover.bgInstance.BgMove().Forget();
        RailMove().Forget();
        Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        BtnAction.actionInstance.GenQTE().Forget();
    }

    private async UniTask RailMove()
    {
        while (flag)
        {
            await UniTask.Yield();
            
            SpeedController().Forget();
            
            rails ??= Instantiate(railPrefab, railPos, Quaternion.identity);
            
            rails1 ??= Instantiate(railPrefab, nextRailPos, Quaternion.identity);
            
            rails.transform.Translate(railMoveVector * (railSpeed * Time.deltaTime));
            rails1.transform.Translate(railMoveVector * (railSpeed * Time.deltaTime));

            if (!(rails1.transform.position.x < -1.8f)) continue;
            var rails2 = Instantiate(railPrefab, nextRailPos, Quaternion.identity);
            Destroy(rails);
            rails = rails1;
            rails1 = rails2;
        }
    }

    private async UniTask SpeedController()
    {
        if (BtnController.ctrlInstance.isAccel)
        {
            railSpeed = 3f;
            BtnController.ctrlInstance.isAccel = false;
        }
        else
        {
            railSpeed = 1f;
        }
        
        await UniTask.WaitForSeconds(0.5f);
    }

    public override void GameStart()
    {
        BGMover.bgInstance.BgMove().Forget();
        RailMove().Forget();
        Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        BtnAction.actionInstance.GenQTE().Forget();
    }

    public override void GetScore()
    {
        var clearTime = BtnAction.actionInstance.clearTime;
    }

    public override void GameEnd()
    {
        flag = false;
        TotalManager.instance.ScoreBoardTest();
    }
}
