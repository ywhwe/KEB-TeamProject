using System;
using UnityEngine;

public class BtnPlayer : MonoBehaviour
{
    public static BtnPlayer playerInstance;
    
    [SerializeField]
    private GameObject playerContainer;
    
    public GameObject playerModel;
    private Transform modelTrans;

    private Vector3 modelPosition = new(0f, 0f, 0f);
    private Quaternion modelAngle = new(0f, 0.7071068f, 0f, 0.7071068f);

    private void Awake()
    {
        playerInstance = this;
    }

    private void Start()
    {
        playerModel = TotalManager.instance.playerPrefab;
        var player = Instantiate(playerModel, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(playerContainer.transform, false);
        
        modelTrans = player.transform;

        modelTrans.transform.localPosition = modelPosition;
        modelTrans.transform.localRotation = modelAngle;
    }
}
