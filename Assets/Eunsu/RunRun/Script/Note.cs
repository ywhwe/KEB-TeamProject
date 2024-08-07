using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Note : MonoBehaviour
{
    private RectTransform _rectTransform;
    
    [HideInInspector] public float noteSpeed = 3f;
    
    private float movePos;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        SpeedChecker(GameManagerRun.instance.getHigher);
        StartCoroutine(MoveNote());
    }

    private void Update()
    {
        if (_rectTransform.position.x > -980f) return;
        
        Destroy(gameObject);
        
        GameManagerRun.instance.noteCount--;
    }

    private IEnumerator MoveNote()
    {
        while (_rectTransform.position.x > -980f)
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

    private void SpeedChecker(int flag)
    {
        switch (flag)
        {
            case 0:
                break;
            case 1:
                noteSpeed = 4f;
                break;
            case 2:
                noteSpeed = 5f;
                break;
            case 3:
                noteSpeed = 7f;
                break;
        }
    }
}
