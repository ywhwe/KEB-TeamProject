using UnityEngine;

public class BtnPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerContainer;
    
    public GameObject playerModel;
    private Transform modelTrans;

    private readonly Vector3 modelPosition = new(0f, 0f, 0f);
    private readonly Vector3 smallModelPosition = new(0f, 0.4f, 0f);
    private readonly Quaternion modelAngle = new(0f, 0.7071068f, 0f, 0.7071068f);

    private void Start()
    {
        playerModel = TotalManager.instance.playerPrefab;
        // if player selection is 2, 3, 4 or 6th character(gekko, herring, muskrat or sparrow), y pos will be 0.4f
        
        var player = Instantiate(playerModel, Vector3.zero, Quaternion.identity, playerContainer.transform);
        // Setting parent later instead of setting while instantiating is generating prefab twice
        
        modelTrans = player.transform;

        modelTrans.transform.localPosition =
            TotalManager.instance.playerPrefabNumber is 2 or 3 or 4 or 6 ? smallModelPosition : modelPosition;
        modelTrans.transform.localRotation = modelAngle;
    }
}
