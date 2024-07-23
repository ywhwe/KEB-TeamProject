using Cysharp.Threading.Tasks;
using UnityEngine;

public class Background : MonoBehaviour
{
    public static Background bgInstance;

    public GameObject backComp;
    public GameObject backgroundPrefab;
    
    private GameObject currentBg;
    private GameObject nextBg;
    
    private Vector3 initPos = new(0f, 0f, 0f);
    private Vector3 nextPos = new(1920f, 0f, 0f);

    private Vector3 movePos = new(-3f, 0f, 0f);

    private Quaternion defaultAngle = new(0f, 0f, 0f, 1f);

    private float moveSpeed = 5f;

    private void Awake()
    {
        bgInstance = this;
    }

    public async UniTask BackgroundMove()
    {
        while (Application.isPlaying)
        {
            await UniTask.Yield();
            
            currentBg ??= Instantiate(backgroundPrefab, initPos, defaultAngle);
            nextBg ??= Instantiate(backgroundPrefab, nextPos, defaultAngle);
            
            currentBg.transform.SetParent(backComp.transform, false);
            nextBg.transform.SetParent(backComp.transform, false);
            
            currentBg.transform.Translate(movePos * (moveSpeed * Time.deltaTime));
            nextBg.transform.Translate(movePos * (moveSpeed * Time.deltaTime));

            if (!(nextBg.transform.position.x < 0.5f)) continue;
            
            var bg = Instantiate(backgroundPrefab, nextPos, defaultAngle);
            
            Destroy(currentBg);

            currentBg = nextBg;
            nextBg = bg;
        }
    }
}
