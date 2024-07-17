using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class BtnController : MonoBehaviour
{
    public static BtnController ctrlInstance;
    
    public GameObject Wbtn;
    public GameObject Abtn;
    public GameObject Sbtn;
    public GameObject Dbtn;

    private Color tempColor = Color.black;
    
    private KeyCode inputKeyCode = KeyCode.None;
    
    private bool isMatch = true;
    public bool IsMatch => isMatch;

    private void Awake()
    {
        ctrlInstance = this;
    }

    void Update()
    {
        isMatch = false;
        SetKey();
        if (inputKeyCode is KeyCode.None) return;
        CompKey();
    }
    
    // stock user input in range WASD
    private void SetKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            inputKeyCode = KeyCode.W;
            Wbtn.GetComponent<Image>().color = Color.red;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            Wbtn.GetComponent<Image>().color = tempColor;
        }
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            inputKeyCode = KeyCode.A;
            Abtn.GetComponent<Image>().color = Color.yellow;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Abtn.GetComponent<Image>().color = tempColor;
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            inputKeyCode = KeyCode.S;
            Sbtn.GetComponent<Image>().color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            Sbtn.GetComponent<Image>().color = tempColor;
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            inputKeyCode = KeyCode.D;
            Dbtn.GetComponent<Image>().color = Color.green;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            Dbtn.GetComponent<Image>().color = tempColor;
        }
    }
    
    // compares generated button and stocked user input
    private async void CompKey()
    {
        if (BtnAction.actionInstance.waitingKeyCode == inputKeyCode)
        {
            inputKeyCode = KeyCode.None;
            isMatch = true;
        }
        else
        {
            inputKeyCode = KeyCode.None;
            await UniTask.WaitForSeconds(1f);
            isMatch = false;
        }
    }
}
