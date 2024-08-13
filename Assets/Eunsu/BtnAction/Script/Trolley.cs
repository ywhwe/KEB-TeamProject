using UnityEngine;

public class Trolley : MonoBehaviour
{
    private RTCObjMover mover;
    
    [SerializeField] private Animator ani;
    
    [Header("Wheels")]
    [SerializeField] private GameObject FrontWheels;
    [SerializeField] private GameObject BackWheels;

    [Header("VFX")]
    public GameObject accelerationSmoke;
    public GameObject decelerationSpark1;
    public GameObject decelerationSpark2;
    
    private static readonly int isFinished = Animator.StringToHash("isFinished");

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        mover = RTCObjMover.RtcObjInstance;
    }

    private void Start()
    {
        RTCObjMover.RtcObjInstance.frontWheels = FrontWheels;
        RTCObjMover.RtcObjInstance.backWheels = BackWheels;
        mover.smoke = accelerationSmoke;
        mover.spark1 = decelerationSpark1;
        mover.spark2 = decelerationSpark2;
    }

    private void Update()
    {
        if (!RTCGameManager.instance.flag) ani.SetBool(isFinished, true);
    }
}
