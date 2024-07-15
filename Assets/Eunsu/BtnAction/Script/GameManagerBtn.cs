using UnityEngine;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;

public class GameManagerBtn : MonoBehaviour
{
    public GameObject railPrefab;
    public GameObject trolleyPrefab;
    [HideInInspector]
    public GameObject trolleyClone;

    private GameObject rails;
    private GameObject rails1;
    private Transform railTrans;
    private Vector3 railMoveVector = new (-3f, 0f, 0f);

    public static GameManagerBtn instance;
    private Vector3 railPos = new (-1.84f, 0.05f, 1.04f);
    private Vector3 nextRailPos = new(10f, 0.05f, 1.04f);
    private Vector3 trolleyPos = new (2f, 0.25f, -0.197f);
    private float railSpeed = 8f;
    private float trolleySpeed = 3f;
    // private Vector3 trolleyMove = new(2f, 0f, 0f);

    private void Awake()
    {
        // When GameManager inheritance Fixed, this should be in StartGame()
        instance = this;
        trolleyClone = Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
    }

    private void Start()
    {
        BGMover.bgInstance.BgMove().Forget();
        GroundMover.groundInstance.GroundMove().Forget();
        RailMove().Forget();
        BtnAction.actionInstance.StartGen(); // When GameManager inheritance Fixed, this should be in StartGame()
    }

    /*private async UniTask RailSet()
    {
        while (Application.isPlaying)
        {
            await UniTask.WaitForSeconds(0.3f);

            railPos.x += railMove;
            railPos = new Vector3(railPos.x, railPos.y, railPos.z);
            Instantiate(railPrefab, railPos, Quaternion.identity);
        }
    }

    private async UniTask MoveForward()
    {
        while (BtnAction.actionInstance.successCount < 10)
        {
            await UniTask.WaitForSeconds(0f);

            /*var vector3 = trolleyClone.transform.position;
            vector3.x += trolleySpeed;
            trolleyClone.transform.position = new Vector3(vector3.x, trolleyClone.transform.position.y,
                trolleyClone.transform.position.z);#1#
            if (!trolleyClone) return;
            trolleyClone.transform.Translate(trolleyMove * trolleySpeed * Time.deltaTime);
        }
    }*/
    
    private async UniTask RailMove()
    {
        while (Application.isPlaying)
        {
            await UniTask.WaitForSeconds(0.1f);
            
            rails ??= Instantiate(railPrefab, railPos, Quaternion.identity);
            
            rails1 ??= Instantiate(railPrefab, nextRailPos, Quaternion.identity);
            
            rails.transform.Translate(railMoveVector * railSpeed * Time.deltaTime);
            rails1.transform.Translate(railMoveVector * railSpeed * Time.deltaTime);

            if (rails.transform.position.x < -10f)
            {
                var rails2 = Instantiate(railPrefab, nextRailPos, Quaternion.identity);

                if (rails1.transform.position.x < -1.8f)
                {
                    Destroy(rails);
                    rails = rails1;
                    rails1 = rails2;
                }
            }
        }
    }
}
