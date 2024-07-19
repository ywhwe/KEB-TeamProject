using UnityEngine;

public class BtnPlayer : MonoBehaviour
{
    public static BtnPlayer playerInstance;
    
    public GameObject playerModel;
    private Transform modelTrans;

    public GameObject playerSpace;

    private Vector3 modelPosition = new(0f, 0f, 0f);
    private Vector3 modelScale = new(150f, 150f, 150f);
    private Quaternion modelAngle = new(0f, 0.5165333f, 0f, 0.8562672f);

    private void Awake()
    {
        playerInstance = this;
        
        var player = Instantiate(playerModel, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(playerSpace.transform, false);
        
        modelTrans = player.transform;

        modelTrans.transform.localPosition = modelPosition;
        modelTrans.transform.localScale = modelScale;
        modelTrans.transform.localRotation = modelAngle;
    }
}
