using UnityEngine;

public class BtnController : MonoBehaviour
{
    private KeyCode inputKeyCode = KeyCode.None;
    
    private bool isMatch = true;
    public bool IsMatch => isMatch;

    private void Awake()
    {
        
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
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            inputKeyCode = KeyCode.A;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            inputKeyCode = KeyCode.S;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            inputKeyCode = KeyCode.D;
        }
    }
    
    // compares generated button and stocked user input
    private void CompKey()
    {
        if (BtnAction.Instance.waitingKeyCode == inputKeyCode)
        {
            inputKeyCode = KeyCode.None;
            isMatch = true;
        }
        else
        {
            inputKeyCode = KeyCode.None;
            isMatch = false;
        }
    }
}
