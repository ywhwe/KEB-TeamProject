using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;

    public KeyCode keyToPress;

    private void Awake()
    {
        GetComponentInChildren<BoxCollider>().enabled = false;
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
            GetComponentInChildren<BoxCollider>().enabled = true;
        }
        
        if (Input.GetKeyUp(keyToPress))
        {
            theSR.sprite = defaultImage;
            GetComponentInChildren<BoxCollider>().enabled = false;
        }
    }
}
