using UnityEngine;
using Cysharp.Threading.Tasks;

public class GroundMover : MonoBehaviour
{
    public static GroundMover groundInstance;
    
    public GameObject groundPrefab;
    
    private GameObject currentGround;
    private GameObject nextGround;
    
    private Vector3 initGroundPos = new (10.62f, 0.1f, -1.4f);
    private Vector3 secGroundPos = new(55.36f, 0.1f, -1.4f);
    private Vector3 nextGroundPos = new (35.36f, 0.1f, -1.4f);
    
    private Vector3 groundMoveVector = new (-3f, 0f, 0f);

    private Quaternion initAngle = new (0f, 0f, 0f, 1f);
    
    private float groundSpeed = 8f;
    private float duration = 0.1f;

    private void Awake()
    {
        groundInstance = this;
    }

    public async UniTask GroundMove()
    {
        while (Application.isPlaying)
        {
            await UniTask.WaitForSeconds(duration);
            
            currentGround ??= Instantiate(groundPrefab, initGroundPos, initAngle);
            
            nextGround ??= Instantiate(groundPrefab, secGroundPos, initAngle);
            
            currentGround.transform.Translate(groundMoveVector * groundSpeed * Time.deltaTime);
            nextGround.transform.Translate(groundMoveVector * groundSpeed * Time.deltaTime);

            if (!(nextGround.transform.position.x < -8.8f)) continue;
            var background = Instantiate(groundPrefab, nextGroundPos, initAngle);
            Destroy(currentGround);
            currentGround = nextGround;
            nextGround = background;
        }
    }
}
