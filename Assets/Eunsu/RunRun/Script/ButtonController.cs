using UnityEngine;
using UnityEngine.UI;
using static System.MathF;

public class ButtonController : MonoBehaviour
{
    public ScoreBoard scoreBoard;

    private Image img;
    
    public Sprite defaultImage;
    public Sprite pressedImage;
    
    private Vector2 hitboxSize;
    
    public KeyCode keyToPress;

    public int noteScore;

    private void Awake()
    {
        hitboxSize.x = 30f;
        hitboxSize.y = 100f;

        img = GetComponent<Image>();
        img.sprite = defaultImage;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            img.sprite = pressedImage;
            var judge = Physics2D.OverlapBox(transform.position, hitboxSize, 0f);
            
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
                        noteScore = 200;
                        break;
                    case < 15:
                        Debug.Log("Great");
                        noteScore = 100;
                        break;
                    default:
                        Debug.Log("Good");
                        noteScore = 50;
                        break;
                }
                
                SoundManager.instance.PlaySound("ClearNote");
                
                Destroy(judge.gameObject);
                NoteController.instance.noteCount--;
                scoreBoard.SetScore(noteScore);
            }
        }
        
        if (Input.GetKeyUp(keyToPress))
        {
            img.sprite = defaultImage;
        }
    }
}
