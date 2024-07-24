using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ObjMover : MonoBehaviour
{
    public static ObjMover ObjInstance;
    private const float Coefficient = 1f;

    [Header("Trolley")]
    
    [Header("Rail")]
    public GameObject railPrefab;
    
    private GameObject rails;
    private GameObject rails1;
    private Transform railTrans;
    private Vector3 railMoveVector = new (-3f, 0f, 0f);
    
    private Vector3 railPos = new (-1.84f, 0.05f, 1.04f);
    private Vector3 nextRailPos = new(10f, 0.05f, 1.04f);
    
    [Header("Background")]
    public GameObject backgroundPrefab;
    
    private GameObject currentBackground;
    private GameObject nextBackground;
    
    private Vector3 initBgPos = new (10.62f, 3.95f, 3f);
    private Vector3 secBgPos = new(55.36f, 3.95f, 3f);
    private Vector3 nextBgPos = new (35.36f, 3.95f, 3f);
    
    private Vector3 bgMoveVector = new (3f, 0f, 0f);
    
    private Quaternion initBgAngle = new (0f, 0.7071068f, -0.7071068f, 0f);
    
    [Header("Ground")]
    public GameObject groundPrefab;
    
    private GameObject currentGround;
    private GameObject nextGround;
    
    private Vector3 initGroundPos = new (10.62f, 0.1f, -1.4f);
    private Vector3 secGroundPos = new(55.36f, 0.1f, -1.4f);
    private Vector3 nextGroundPos = new (35.36f, 0.1f, -1.4f);
    
    private Vector3 groundMoveVector = new (-3f, 0f, 0f);
    
    private Quaternion initGroundAngle = new (0f, 0f, 0f, 1f);
    
    private float objSpeed = 1f;

    [HideInInspector] public float angle = 3f;

    private GameObject[] trolley;

    private float timer = 0;

    private void Awake()
    {
        ObjInstance = this;
    }

    public async UniTask RailMove()
    {
        while (GameManagerBtn.instance.flag)
        {
            await UniTask.Yield();
            
            rails ??= Instantiate(railPrefab, railPos, Quaternion.identity);
            
            rails1 ??= Instantiate(railPrefab, nextRailPos, Quaternion.identity);
            
            rails.transform.Translate(railMoveVector * (objSpeed * Time.deltaTime));
            rails1.transform.Translate(railMoveVector * (objSpeed * Time.deltaTime));

            if (!(rails1.transform.position.x < -1.8f)) continue;
            var rails2 = Instantiate(railPrefab, nextRailPos, Quaternion.identity);
            Destroy(rails);
            rails = rails1;
            rails1 = rails2;
        }
    }

    public async UniTask BgMove()
    {
        while (GameManagerBtn.instance.flag)
        {
            await UniTask.Yield();
            
            currentBackground ??= Instantiate(backgroundPrefab, initBgPos, initBgAngle);
            currentGround ??= Instantiate(groundPrefab, initGroundPos, initGroundAngle);
            
            nextBackground ??= Instantiate(backgroundPrefab, secBgPos, initBgAngle);
            nextGround ??= Instantiate(groundPrefab, secGroundPos, initGroundAngle);
            
            currentBackground.transform.Translate(bgMoveVector * (objSpeed * Time.deltaTime));
            currentGround.transform.Translate(groundMoveVector * (objSpeed * Time.deltaTime));
            
            nextBackground.transform.Translate(bgMoveVector * (objSpeed * Time.deltaTime));
            nextGround.transform.Translate(groundMoveVector * (objSpeed * Time.deltaTime));
            
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
    
    public async UniTask SpeedController()
    {
        /*var tempVec1 = railMoveVector;
        var tempVec2 = bgMoveVector;
        var tempVec3 = groundMoveVector;*/
        
        if (GameManagerBtn.instance.IsMatch)
        {
            while (timer < MathF.PI)
            {
                timer += Time.deltaTime;
                objSpeed = 5f * MathF.Sin(timer) + Coefficient;
            }

            UniTask.Delay(4000);
            /*railMoveVector = new Vector3(-10f, 0f, 0f);
            bgMoveVector = new Vector3(10f, 0f, 0f);
            groundMoveVector = new Vector3(-10f, 0f, 0f);

            await UniTask.Delay(1000);

            railMoveVector = tempVec1;
            bgMoveVector = tempVec2;
            groundMoveVector = tempVec3;*/
        }
    }
    
    public async UniTask Spin(GameObject hmm)
    {
        while (GameManagerBtn.instance.successCount < 10)
        {
            await UniTask.Yield();
            
            hmm.transform.Find("FrontWheels").Rotate(Vector3.back, angle, Space.Self);
            hmm.transform.Find("BackWheels").Rotate(Vector3.back, angle, Space.Self);
        }
    }
}