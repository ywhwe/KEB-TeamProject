using UnityEngine;
using Cysharp.Threading.Tasks;

public class GameManagerBtn : MonoBehaviour
{
    public GameObject railPrefab;
    public GameObject trolleyPrefab;
    [HideInInspector]
    public GameObject trolleyClone;

    public static GameManagerBtn instance;
    private Vector3 railPos = new (-1.84f, 0.05f, 1.04f);
    private Vector3 trolleyPos = new (2f, 0.25f, -0.197f);
    private float railMove = 3f;
    private float trolleySpeed = 3f;
    private Vector3 trolleyMove = new(2f, 0f, 0f);

    private void Awake()
    {
        // When GameManager inheritance Fixed, this should be in StartGame()
        // RailSet().Forget();
        Instantiate(railPrefab, railPos, Quaternion.identity);
        trolleyClone = Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
        // MoveForward().Forget();
    }

    private void Start()
    {
        BGMover.bgInstance.BgMove().Forget();
        GroundMover.groundInstance.GroundMove().Forget();
        BtnAction.actionInstance.StartGen(); // When GameManager inheritance Fixed, this should be in StartGame()
    }

    private async UniTask RailSet()
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
                trolleyClone.transform.position.z);*/
            if (!trolleyClone) return;
            trolleyClone.transform.Translate(trolleyMove * trolleySpeed * Time.deltaTime);
        }
    }
}
