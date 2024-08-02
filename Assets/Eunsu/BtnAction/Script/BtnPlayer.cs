using UnityEngine;

public class BtnPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerContainer;
    
    public GameObject playerModel;
    private Transform modelTrans;

    private readonly Vector3 modelPosition = new(0f, 0f, 0f);
    private readonly Quaternion modelAngle = new(0f, 0.7071068f, 0f, 0.7071068f);

    private void Start()
    {
        playerModel = TotalManager.instance.playerPrefab;
        var player = Instantiate(playerModel, Vector3.zero, Quaternion.identity, playerContainer.transform);
        // Setting parent later instead of setting while instantiating is generating prefab twice
        
        modelTrans = player.transform;

        modelTrans.transform.localPosition = modelPosition;
        modelTrans.transform.localRotation = modelAngle;
    }
}
