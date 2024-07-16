using UnityEngine;
using Cysharp.Threading.Tasks;

public class BGMover : MonoBehaviour
{
    public static BGMover bgInstance;
    
    public GameObject backgroundPrefab;
    public GameObject groundPrefab;
    
    private GameObject currentBackground;
    private GameObject nextBackground;
    
    private GameObject currentGround;
    private GameObject nextGround;
    
    private Vector3 initBgPos = new (10.62f, 3.95f, 3f);
    private Vector3 secBgPos = new(55.36f, 3.95f, 3f);
    private Vector3 nextBgPos = new (35.36f, 3.95f, 3f);
    
    private Vector3 initGroundPos = new (10.62f, 0.1f, -1.4f);
    private Vector3 secGroundPos = new(55.36f, 0.1f, -1.4f);
    private Vector3 nextGroundPos = new (35.36f, 0.1f, -1.4f);
    
    private Vector3 bgMoveVector = new (3f, 0f, 0f);
    private Vector3 groundMoveVector = new (-3f, 0f, 0f);
    
    private Quaternion initBgAngle = new (0f, 0.7071068f, -0.7071068f, 0f);
    private Quaternion initGroundAngle = new (0f, 0f, 0f, 1f);
    
    private float bgSpeed = 8f;
    private float duration = 0.1f;

    private void Awake()
    {
        bgInstance = this;
    }

    public async UniTask BgMove()
    {
        while (Application.isPlaying)
        {
            await UniTask.WaitForSeconds(duration);
            
            currentBackground ??= Instantiate(backgroundPrefab, initBgPos, initBgAngle);
            currentGround ??= Instantiate(groundPrefab, initGroundPos, initGroundAngle);
            
            nextBackground ??= Instantiate(backgroundPrefab, secBgPos, initBgAngle);
            nextGround ??= Instantiate(groundPrefab, secGroundPos, initGroundAngle);
            
            currentBackground.transform.Translate(bgMoveVector * (bgSpeed * Time.deltaTime));
            currentGround.transform.Translate(groundMoveVector * (bgSpeed * Time.deltaTime));
            
            nextBackground.transform.Translate(bgMoveVector * (bgSpeed * Time.deltaTime));
            nextGround.transform.Translate(groundMoveVector * (bgSpeed * Time.deltaTime));
            
            if (!(nextBackground.transform.position.x < -8.7f && nextGround.transform.position.x < -8.7f)) continue;
            
            var background = Instantiate(backgroundPrefab, nextBgPos, initBgAngle);
            var ground = Instantiate(groundPrefab, nextGroundPos, initGroundAngle);

            Destroy(currentBackground);
            Destroy(currentGround);
            
            currentBackground = nextBackground;
            currentGround = nextGround;
            
            nextBackground = background;
            nextGround = ground;
        }
    }
}