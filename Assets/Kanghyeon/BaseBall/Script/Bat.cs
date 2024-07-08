using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bat : MonoBehaviour
{
    private Animator ani;

    public BallSpawner ballsapwner;
    
    private static readonly int Swing = Animator.StringToHash("Swing");
    // Start is called before the first frame update
    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ani.SetTrigger(Swing);
            ballsapwner.Check();
        }
    }

  
}
