using UnityEngine;
using Cysharp.Threading.Tasks;

public class GroundMover : MonoBehaviour
{
    public static GroundMover groundInstance;
    public GameObject ground;
    private Transform groundTrans;
    private Vector3 groundMoveVector = new (-3f, 0f, 0f);
    private float groundSpeed = 8f;
    private float duration = 0.1f;

    private bool flag = true;

    private void Awake()
    {
        groundInstance = this;
        groundTrans = ground.transform;
    }

    public async UniTask GroundMove()
    {
        while (flag)
        {
            await UniTask.WaitForSeconds(duration);
            groundTrans.Translate(groundMoveVector * groundSpeed * Time.deltaTime);
            
            if (groundTrans.position.x < -47f) flag = false;
        }
    }
}
