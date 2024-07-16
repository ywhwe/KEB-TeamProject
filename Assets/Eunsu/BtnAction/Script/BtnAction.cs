using System.Threading;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Random = UnityEngine.Random;

public class BtnAction : MonoBehaviour
{
    private Animator ani;
    public TextMeshProUGUI qteBtn;
    public BtnController btnController;
    
    public static BtnAction actionInstance;
    
    [HideInInspector]
    public KeyCode waitingKeyCode = KeyCode.None;
    
    private float rand;
    public int successCount;

    [HideInInspector]
    public float clearTime = 0;
    
    private static readonly int Gen = Animator.StringToHash("Gen");
    private static readonly int isWait = Animator.StringToHash("isWait");

    void Awake()
    {
        // When GameManager inheritance Fixed, this should be in StartGame()
        actionInstance = this;
        ani = GetComponent<Animator>();
        successCount = 0;
    }

    private void Update()
    {
        clearTime += Time.deltaTime;
    }

    public void StartGen()
    {
        GenQTE().Forget();
    }

    // button generator for coroutine
    public async UniTask GenQTE()
    {
        while (successCount < 10) // if successCount bigger than setting value stops coroutine
        {
            // if IsMatch is false, suspends coroutine 'til it is true
            await UniTask.WaitUntil(() => btnController.IsMatch);
            
            rand = Random.Range(0, 100);
            BtnControl(rand);

            if (!btnController.IsMatch) continue; // user input matched with QTE buttons increase successCount
            ani.SetBool(isWait, false);
            successCount++;
        }
        
        Debug.Log("Buttons are all cleared in " + clearTime + " sec");
    }
    
    // randomly decides next presenting button; depending on random value in range 0-100
    private void BtnControl(float random)
    {
        switch (random)
        {
            case >= 0 and < 25 :
                qteBtn.text = "[ W ]";
                waitingKeyCode = KeyCode.W;
                ani.SetTrigger(Gen);
                break;
            case >= 25 and < 50 :
                qteBtn.text = "[ A ]";
                waitingKeyCode = KeyCode.A;
                ani.SetTrigger(Gen);
                break;
            case >= 50 and <75 :
                qteBtn.text = "[ S ]";
                waitingKeyCode = KeyCode.S;
                ani.SetTrigger(Gen);
                break;
            case >= 75 and < 100 :
                qteBtn.text = "[ D ]";
                waitingKeyCode = KeyCode.D;
                ani.SetTrigger(Gen);
                break;
        }
        ani.SetBool(isWait, true);
    }
}
