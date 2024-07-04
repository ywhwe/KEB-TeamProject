using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;
    
    private Vector2 buttonSize;
    
    public KeyCode keyToPress;

    private void Awake()
    {
        buttonSize.x = 30f;
        buttonSize.y = 100f;
    }

    void Start()
    {
        theSR = GetComponent<SpriteRenderer>();
        theSR.sprite = defaultImage;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theSR.sprite = pressedImage;
            var judge = Physics2D.OverlapBox(transform.position, buttonSize, 0f);
            if (judge is null)
            {
                Debug.Log("null");
            }
            else if (judge.CompareTag("JudgeLine"))
            {
                Destroy(judge.gameObject);
            }
        }
        
        if (Input.GetKeyUp(keyToPress))
        {
            theSR.sprite = defaultImage;
        }
    }
}
