using System.Collections;
using UnityEngine;

public class Note : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    [HideInInspector] public float noteSpeed = 5f;
    
    private float movePos;

    private float durationTime = 0f;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        if (NoteController.instance.noteNumber > 50) noteSpeed = 7f;
        StartCoroutine(MoveNote());
    }

    private void Update()
    {
        durationTime += Time.deltaTime;
        
        if (durationTime < 12f) return;
        
        Destroy(gameObject);
        
        NoteController.instance.noteCount--;
    }

    private IEnumerator MoveNote()
    {
        while (durationTime < 12f)
        {
            yield return null;
            
            var vector2 = _rectTransform.anchoredPosition;
            vector2.x -= noteSpeed;
            
            _rectTransform.anchoredPosition = vector2;
            
            _rectTransform.anchoredPosition 
                = new Vector2(_rectTransform.anchoredPosition.x, _rectTransform.anchoredPosition.y);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(gameObject);
    }
}
