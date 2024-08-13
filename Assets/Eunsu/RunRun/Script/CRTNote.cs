using System.Collections;
using UnityEngine;

public class CRTNote : MonoBehaviour
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
        StartCoroutine(MoveNote());
    }

    private void Update()
    {
        SpeedChecker(CRTGameManager.instance.getHigher);
        if (_rectTransform.position.x > -520f) return;
        
        Destroy(gameObject);
        
        CRTGameManager.instance.noteCount--;
    }

    private IEnumerator MoveNote()
    {
        while (_rectTransform.position.x > -520f)
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
