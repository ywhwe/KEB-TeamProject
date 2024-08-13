using UnityEngine;

public class CRTPlayer : MonoBehaviour
{
    [SerializeField]
    private GameObject playerContainer;
    
    public GameObject playerModel;
    private Transform modelTrans;

    private readonly Vector3 modelPosition = new(0f, 0f, 0f);
    private readonly Vector3 modelScale = new(150f, 150f, 150f);
    private readonly Quaternion modelAngle = new(0f, 0.7071068f, 0f, 0.7071068f);

    private void Start() // Make this as identical method
    {
        playerModel = TotalManager.instance.playerPrefab;
        var player = Instantiate(playerModel, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(playerContainer.transform);
        
        modelTrans = player.transform;

        modelTrans.transform.localPosition = modelPosition;
        modelTrans.transform.localScale = modelScale;
        modelTrans.transform.localRotation = modelAngle;
    }
}
