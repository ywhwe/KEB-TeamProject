using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private NoteController nController;
    private ButtonController BController;
    private RectTransform _rectTransform;
    private WaitForSeconds hmm = new WaitForSeconds(0.01f);
    private float noteSpeed = 1f;
    private float movePos;
    
    private float durationTime = 0f;

    public bool canBePressed = false;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(MoveNote());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S))
        {
            if (canBePressed)
            {
                gameObject.SetActive(false);
            }
        }
        durationTime += Time.deltaTime;
        if (!(durationTime > 10f)) return;
        Destroy(gameObject);
        Debug.Log("note destroyed");
    }

    private IEnumerator MoveNote()
    {
        while (true)
        {
            yield return hmm;
            
            movePos -= noteSpeed;
            _rectTransform.anchoredPosition = new Vector2(movePos, _rectTransform.anchoredPosition.y);
            // gameObject.transform.Translate(-1f * (noteSpeed * Time.deltaTime), 0f, 0f);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("JudgeLine"))
        {
            canBePressed = true;
            // controller.DestroyNotes(gameObject);
        }
    }
}
