using UnityEngine;
using Cysharp.Threading.Tasks;

public class BtnAction : MonoBehaviour
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
        if (GameManagerBtn.instance.isGen)
        {
            genAni.SetTrigger(Gen);
        }
        
        if (!GameManagerBtn.instance.isLegal)
        {
            blockBox.SetActive(true);
            await UniTask.WaitForSeconds(1f);
            blockBox.SetActive(false);
        }

        GameManagerBtn.instance.isGen = false;
    }
}
