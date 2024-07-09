using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class BtnAction : MonoBehaviour
{
    private Animator ani;
    public TextMeshProUGUI qteBtn;
    public static BtnAction Instance;
    public BtnController btnController;
    private float disabledTime = 0.5f;
    
    [HideInInspector]
    public KeyCode waitingKeyCode = KeyCode.None;
    private float rand;
    private int successCount;
    private static readonly int Gen = Animator.StringToHash("Gen");
    private static readonly int isWait = Animator.StringToHash("isWait");

    void Awake()
    {
        Instance = this;
        ani = GetComponent<Animator>();
        successCount = 0;
    }

    private void Start()
    {
        StartCoroutine(GenQTE());
    }

    // button generator for coroutine
    private IEnumerator GenQTE()
    {
        while (successCount < 10) // if successCount bigger than setting value stops coroutine
        {
            // if IsMatch is false, suspends coroutine 'til it is true
            yield return new WaitUntil(() => btnController.IsMatch);
            
            rand = Random.Range(0, 100);
            BtnControl(rand);
            
            if (btnController.IsMatch) // user input matched with QTE buttons increase successCount
            {
                ani.SetBool(isWait, false);
                successCount++;
            }
        }
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
