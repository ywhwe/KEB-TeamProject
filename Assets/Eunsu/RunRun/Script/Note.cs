using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Note : MonoBehaviour
{
    public RectTransform note;
    public Vector2 pos;

    private void Awake()
    {
        note = GetComponent<RectTransform>();
    }

    public void SetPos()
    {
        note.anchoredPosition = pos;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        
    }
}
