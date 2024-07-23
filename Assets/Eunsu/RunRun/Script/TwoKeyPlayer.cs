using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class TwoKeyPlayer : MonoBehaviour
{
    public static TwoKeyPlayer playerInstance;
    
    [SerializeField]
    private GameObject playerContainer;
    
    public GameObject playerModel;
    private Transform modelTrans;

    private Vector3 modelPosition = new(0f, 0f, 0f);
    private Vector3 modelScale = new(150f, 150f, 150f);
    private Quaternion modelAngle = new(0f, 0.5165333f, 0f, 0.8562672f);

    private Animator modelAni;
    private static readonly int IsTwoKeyWMove = Animator.StringToHash("isTwoKeyWMove");
    private static readonly int IsTwoKeySMove = Animator.StringToHash("isTwoKeySMove");

    private void Awake()
    {
        playerInstance = this;
    }

    private void Start() // Make this as identical method
    {
        playerModel = TotalManager.instance.playerPrefab;
        var player = Instantiate(playerModel, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(playerContainer.transform);

        modelAni = player.GetComponent<Animator>();
        
        modelTrans = player.transform;

        modelTrans.transform.localPosition = modelPosition;
        modelTrans.transform.localScale = modelScale;
        modelTrans.transform.localRotation = modelAngle;
    }

    public async UniTask KeyInteraction()
    {
        while (Application.isPlaying)
        {
            await UniTask.Yield();

            if (Input.GetKeyDown(KeyCode.W))
            {
                modelAni.SetBool(IsTwoKeyWMove, true);
            }
            
            modelAni.SetBool(IsTwoKeyWMove, false);
            
            if (Input.GetKeyDown(KeyCode.S))
            {
                modelAni.SetBool(IsTwoKeySMove, true);
            }
            
            modelAni.SetBool(IsTwoKeySMove, false);
        }
    }
}
