using UnityEngine;

public class BtnAction : MonoBehaviour
{
    private Animator ani;
    public GameManagerBtn gameManagerBtn;
    
    private static readonly int Gen = Animator.StringToHash("Gen");

    private void Awake()
    {
        ani = GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameManagerBtn.isGen)
        {
            ani.SetTrigger(Gen);
        }

        gameManagerBtn.isGen = false;
    }
}
