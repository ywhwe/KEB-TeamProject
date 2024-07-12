using UnityEditor.Scripting;
using UnityEngine;
using static System.MathF;

public class ButtonController : MonoBehaviour
{
    public ScoreBoard scoreBoard;
    private SpriteRenderer theSR;
    public Sprite defaultImage;
    public Sprite pressedImage;
    
    private Vector2 buttonSize;
    
    public KeyCode keyToPress;

    public int noteScore;

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
    
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theSR.sprite = pressedImage;
            var judge = Physics2D.OverlapBox(transform.position, buttonSize, 0f);
            if (judge is null)
            {
                // When Note Missed
            }
            else if (judge.CompareTag("JudgeLine")) // When Note Hit
            {
                var dist = gameObject.transform.position - judge.transform.position;
                var distance = Abs(dist.x);
                switch (distance)
                {
                    case < 2:
                        Debug.Log("Perfect");
                        break;
                    case < 15:
                        Debug.Log("Great");
                        break;
                    default:
                        Debug.Log("Good");
                        break;
                }
                Destroy(judge.gameObject);
                NoteController.instance.noteCount--;
                scoreBoard.SetScore(noteScore);
                Debug.Log("Note: " + NoteController.instance.noteCount);
                
            }
        }
        
        if (Input.GetKeyUp(keyToPress))
        {
            theSR.sprite = defaultImage;
        }
    }
}
