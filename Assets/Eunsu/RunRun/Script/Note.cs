using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    private RectTransform _rectTransform;
    private WaitForSeconds hmm = new(0.01f);
    private float noteSpeed = 3f;
    private float movePos;

    private float durationTime = 0f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    void Start()
    {
        StartCoroutine(MoveNote());

    }

    void Update()
    {
        durationTime += Time.deltaTime;
        if (durationTime < 10f) return;
        Destroy(gameObject);
        NoteController.instance.noteCount--;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("");
        Destroy(gameObject);
    }
}
