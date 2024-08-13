using UnityEngine;
using Cysharp.Threading.Tasks;

public class RTCGenAnimator : MonoBehaviour
{
    private Animator genAni;
    public GameObject blockBox;
    
    private static readonly int Gen = Animator.StringToHash("Gen");

    private void Awake()
    {
        genAni = GetComponent<Animator>();
    }

    private async void Update()
    {
        if (RTCGameManager.instance.isGen)
        {
            genAni.SetTrigger(Gen);
        }
        
        if (!RTCGameManager.instance.isLegal)
        {
            blockBox.SetActive(true);
            await UniTask.WaitForSeconds(1f);
            blockBox.SetActive(false);
        }

        RTCGameManager.instance.isGen = false;
    }
}
