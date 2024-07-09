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
    
    [HideInInspector]
    public KeyCode waitingKeyCode = KeyCode.None;
    private float rand;
    private float timer;
    private float timeLimit = 5f;
    private static readonly int Gen = Animator.StringToHash("Gen");
    private static readonly int isWait = Animator.StringToHash("isWait");

    void Awake()
    {
        Instance = this;
        ani = GetComponent<Animator>();
    }

    private void Start()
    {
        StartCoroutine(GenQTE());
    }

    // button generator for coroutine
    private IEnumerator GenQTE()
    {
        while (timer < timeLimit)
        {
            // if IsMatch is false, suspends coroutine 'til it is true
            yield return new WaitUntil(() => btnController.IsMatch);

            timer += Time.deltaTime;
            
            rand = Random.Range(0, 100);
            BtnControl(rand);
            
            if (btnController.IsMatch) ani.SetBool(isWait, false);
        }
    }
    
    // randomly decides next presenting button
    private void BtnControl(float random)
    {
        switch (random)
        {
            case >= 0 and < 25 :
                qteBtn.text = "[ W ]";
                waitingKeyCode = KeyCode.W;
                Debug.Log(waitingKeyCode);
                ani.SetTrigger(Gen);
                break;
            case >= 25 and < 50 :
                qteBtn.text = "[ A ]";
                waitingKeyCode = KeyCode.A;
                Debug.Log(waitingKeyCode);
                ani.SetTrigger(Gen);
                break;
            case >= 50 and <75 :
                qteBtn.text = "[ S ]";
                waitingKeyCode = KeyCode.S;
                Debug.Log(waitingKeyCode);
                ani.SetTrigger(Gen);
                break;
            case >= 75 and < 100 :
                qteBtn.text = "[ D ]";
                waitingKeyCode = KeyCode.D;
                Debug.Log(waitingKeyCode);
                ani.SetTrigger(Gen);
                break;
        }
        ani.SetBool(isWait, true);
    }
}
