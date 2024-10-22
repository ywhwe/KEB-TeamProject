using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class RTCObjMover : MonoBehaviour
{
    public static RTCObjMover RtcObjInstance;
    private const float Coefficient = 7f;
    
    [Header("Trolley")]
    [HideInInspector] public GameObject frontWheels; 
    [HideInInspector] public GameObject backWheels;
    [HideInInspector] public GameObject smoke;
    [HideInInspector] public GameObject spark1;
    [HideInInspector] public GameObject spark2;
    
    [Header("Rail")]
    public GameObject railPrefab;
    
    private GameObject rails;
    private GameObject rails1;
    private Transform railTrans;
    private readonly Vector3 railMoveVector = new (-3f, 0f, 0f);
    
    private readonly Vector3 railPos = new (-1.84f, 0.15f, 1.04f);
    private readonly Vector3 nextRailPos = new(10f, 0.15f, 1.04f);
    
    [Header("Background")]
    public GameObject backgroundPrefab;
    
    private GameObject currentBackground;
    private GameObject nextBackground;
    
    private readonly Vector3 initBgPos = new (10.62f, 3.95f, 3f);
    private readonly Vector3 secBgPos = new(55.36f, 3.95f, 3f);
    private readonly Vector3 nextBgPos = new (35.36f, 3.95f, 3f);
    
    private readonly Vector3 bgMoveVector = new (3f, 0f, 0f);
    
    private readonly Quaternion initBgAngle = new (0f, 0.7071068f, -0.7071068f, 0f);
    
    [Header("Ground")]
    public GameObject groundPrefab;
    
    private GameObject currentGround;
    private GameObject nextGround;
    
    private readonly Vector3 initGroundPos = new (10.62f, 0.1f, -1.4f);
    private readonly Vector3 secGroundPos = new(55.36f, 0.1f, -1.4f);
    private readonly Vector3 nextGroundPos = new (35.36f, 0.1f, -1.4f);
    
    private readonly Vector3 groundMoveVector = new (-3f, 0f, 0f);
    
    private readonly Quaternion initGroundAngle = new (0f, 0f, 0f, 1f);

    private float objSpeed = 1f;

    private float timer;

    private void Awake()
    {
        RtcObjInstance = this;
        timer = 0f;
    }

    public async UniTask RailMove()
    {
        while (RTCGameManager.instance.flag)
        {
            await UniTask.Yield();
            
            rails ??= Instantiate(railPrefab, railPos, Quaternion.identity);
            
            rails1 ??= Instantiate(railPrefab, nextRailPos, Quaternion.identity);
            
            rails?.transform.Translate(railMoveVector * (objSpeed * Time.deltaTime));
            rails1?.transform.Translate(railMoveVector * (objSpeed * Time.deltaTime));

            if (!(rails1?.transform.position.x < -1.8f)) continue;
            var rails2 = Instantiate(railPrefab, nextRailPos, Quaternion.identity);
            Destroy(rails);
            rails = rails1;
            rails1 = rails2;
        }
    }

    public async UniTask BgMove()
    {
        while (RTCGameManager.instance.flag)
        {
            await UniTask.Yield();
            
            currentBackground ??= Instantiate(backgroundPrefab, initBgPos, initBgAngle);
            currentGround ??= Instantiate(groundPrefab, initGroundPos, initGroundAngle);
            
            nextBackground ??= Instantiate(backgroundPrefab, secBgPos, initBgAngle);
            nextGround ??= Instantiate(groundPrefab, secGroundPos, initGroundAngle);
            
            currentBackground?.transform.Translate(bgMoveVector * (objSpeed * Time.deltaTime));
            currentGround?.transform.Translate(groundMoveVector * (objSpeed * Time.deltaTime));
            
            nextBackground?.transform.Translate(bgMoveVector * (objSpeed * Time.deltaTime));
            nextGround?.transform.Translate(groundMoveVector * (objSpeed * Time.deltaTime));
            
            if (!(nextBackground?.transform.position.x < -8.7f && nextGround?.transform.position.x < -8.7f)) continue;
            
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
    
    // Background's speed is varied along the sine wave
    public async UniTask AccelerationSpeed()
    {
        smoke?.SetActive(true);
        
        while (timer < 1f)
        {
            timer += Time.deltaTime;
            objSpeed = Coefficient * MathF.Sin(timer * Mathf.PI) + 3;
            
            await UniTask.Yield();
        }
        
        smoke?.SetActive(false);
        
        objSpeed = 3f;
        timer = 0f;
    }
    
    public async UniTask DecelerationSpeed()
    {
        spark1?.SetActive(true);
        spark2?.SetActive(true);

        objSpeed = 1f;
        await UniTask.WaitForSeconds(1f);
        
        spark1?.SetActive(false);
        spark2?.SetActive(false);
        
        objSpeed = 3f;
        timer = 0f;
    }
    
    // Only used for spin wheels of trolley
    public async UniTask Spin()
    {
        while (RTCGameManager.instance.successCount < RTCGameManager.NumberOfButtons)
        {
            await UniTask.Yield();
            
            frontWheels?.transform.Rotate(Vector3.back, objSpeed * 0.5f, Space.Self);
            backWheels?.transform.Rotate(Vector3.back, objSpeed * 0.5f, Space.Self);
        }
    }
}