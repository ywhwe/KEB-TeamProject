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

    private Color wColor, aColor, sColor, dColor;
    
    [HideInInspector]
    public KeyCode inputKeyCode = KeyCode.None;

    private void Awake()
    {
        ctrlInstance = this; 
        wColor = Wbtn.GetComponent<Image>().color;
        aColor = Abtn.GetComponent<Image>().color;
        sColor = Sbtn.GetComponent<Image>().color;
        dColor = Dbtn.GetComponent<Image>().color;
    }
    
    // Stocks user input in range WASD and change color of buttons
    public async void SetKey()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            inputKeyCode = KeyCode.W;
            wColor = Color.red;
            await UniTask.WaitForSeconds(0.3f);
            wColor = tempColor;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            inputKeyCode = KeyCode.A;
            aColor = Color.yellow;
            await UniTask.WaitForSeconds(0.3f);
            aColor = tempColor;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            inputKeyCode = KeyCode.S;
            sColor = Color.blue;
            await UniTask.WaitForSeconds(0.3f);
            sColor = tempColor;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            inputKeyCode = KeyCode.D;
            dColor = Color.green;
            await UniTask.WaitForSeconds(0.3f);
            dColor = tempColor;
        }
    }
}
