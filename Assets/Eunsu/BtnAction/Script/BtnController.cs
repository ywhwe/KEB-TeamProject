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
    public void SetKey()
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
}
