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
    
    [HideInInspector]
    public KeyCode inputKeyCode = KeyCode.None;

    private void Awake()
    {
        ctrlInstance = this;
    }
    
    // Stocks user input in range WASD and change color of buttons
    public async void SetKey()
    {
        if (Input.GetKey(KeyCode.W))
        {
            inputKeyCode = KeyCode.W;
            Wbtn.GetComponent<Image>().color = Color.red;
            await UniTask.WaitForSeconds(0.3f);
            Wbtn.GetComponent<Image>().color = tempColor;
        }
/*
        if (Input.GetKeyUp(KeyCode.W))
        {
            Wbtn.GetComponent<Image>().color = tempColor;
        }
*/
        if (Input.GetKey(KeyCode.A))
        {
            inputKeyCode = KeyCode.A;
            Abtn.GetComponent<Image>().color = Color.yellow;
            await UniTask.WaitForSeconds(0.3f);
            Abtn.GetComponent<Image>().color = tempColor;
        }
/*
        if (Input.GetKeyUp(KeyCode.A))
        {
            Abtn.GetComponent<Image>().color = tempColor;
        }
*/
        if (Input.GetKey(KeyCode.S))
        {
            inputKeyCode = KeyCode.S;
            Sbtn.GetComponent<Image>().color = Color.blue;
            await UniTask.WaitForSeconds(0.3f);
            Sbtn.GetComponent<Image>().color = tempColor;
        }
/*
        if (Input.GetKeyUp(KeyCode.S))
        {
            Sbtn.GetComponent<Image>().color = tempColor;
        }
*/
        if (Input.GetKey(KeyCode.D))
        {
            inputKeyCode = KeyCode.D;
            Dbtn.GetComponent<Image>().color = Color.green;
            await UniTask.WaitForSeconds(0.3f);
            Dbtn.GetComponent<Image>().color = tempColor;
        }
/*
        if (Input.GetKeyUp(KeyCode.D))
        {
            Dbtn.GetComponent<Image>().color = tempColor;
        }
*/        
    }
}
