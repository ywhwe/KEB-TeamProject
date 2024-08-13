using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class RTCButtonController : MonoBehaviour
{
    public static RTCButtonController ctrlInstance;
    
    public GameObject Wbtn;
    public GameObject Abtn;
    public GameObject Sbtn;
    public GameObject Dbtn;

    private static readonly int PressedW = Animator.StringToHash("PressedW");
    private static readonly int PressedA = Animator.StringToHash("PressedA");
    private static readonly int PressedS = Animator.StringToHash("PressedS");
    private static readonly int PressedD = Animator.StringToHash("PressedD");

    private Animator aniW;
    private Animator aniA;
    private Animator aniS;
    private Animator aniD;
    
    [HideInInspector]
    public KeyCode inputKeyCode = KeyCode.None;

    private void Awake()
    {
        ctrlInstance = this;
        
        aniW = Wbtn.GetComponent<Animator>();
        aniA = Abtn.GetComponent<Animator>();
        aniS = Sbtn.GetComponent<Animator>();
        aniD = Dbtn.GetComponent<Animator>();
    }
    
    // Stocks user input in range WASD and change color of buttons
    public void SetKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            inputKeyCode = KeyCode.W;
            
            aniW.SetTrigger(PressedW);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            inputKeyCode = KeyCode.A;
            
            aniA.SetTrigger(PressedA);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            inputKeyCode = KeyCode.S;
            
            aniS.SetTrigger(PressedS);
        }

        if (!Input.GetKeyDown(KeyCode.D)) return;
        inputKeyCode = KeyCode.D;
            
        aniD.SetTrigger(PressedD);
    }
}
