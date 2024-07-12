using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BGMover : MonoBehaviour
{
    public static BGMover bgInstance;
    public GameObject background;
    private Transform bgTrans;
    private Vector3 bgMoveVector = new (3f, 0f, 0f);
    private float bgSpeed = 8f;
    private float duration = 0.1f;

    private bool flag = true;

    private void Awake()
    {
        bgInstance = this;
        bgTrans = background.transform;
    }

    public async UniTask BgMove()
    {
        while (flag)
        {
            await UniTask.WaitForSeconds(duration);
            bgTrans.Translate(bgMoveVector * bgSpeed * Time.deltaTime);
            
            if (bgTrans.position.x < -47f) flag = false;
        }
    }
}
