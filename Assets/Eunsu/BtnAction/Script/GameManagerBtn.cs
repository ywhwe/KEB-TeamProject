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

    private void Awake()
    {
        instance = this; 
    }

    private void Start()
    {
        // These will be deleted in build version
        BGMover.bgInstance.BgMove().Forget();
        RailMove().Forget();
        Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        BtnAction.actionInstance.GenQTE().Forget();
    }
    
    // Make rails move backward
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
    
    // Controls rail speed. When player input is correct, rail moves faster
    private async UniTask SpeedController()
    {
        var tempVec1 = railMoveVector;
        
        if (BtnController.ctrlInstance.IsMatch)
        {
            railMoveVector = new Vector3(-10f, 0f, 0f);
            
            await UniTask.Delay(1000);

            railMoveVector = tempVec1;
        }
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
        var clearTime = BtnAction.actionInstance.score;
    }

    public override void GameEnd()
    {
        flag = false;
        TotalManager.instance.ScoreBoardTest();
    }
}
