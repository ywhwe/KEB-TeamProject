using UnityEngine;
using static NoteController;

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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            theSR.sprite = pressedImage;
            var judge = Physics2D.OverlapBox(transform.position, buttonSize, 0f);
            if (judge is null)
            {
                Debug.Log("Note Missed");
            }
            else if (judge.CompareTag("JudgeLine"))
            {
                Destroy(judge.gameObject);
                NoteController.instance.noteCount--;
                scoreBoard.SetScore(noteScore);
                Debug.Log("Note: " + instance.noteCount);
                
            }
        }
        
        if (Input.GetKeyUp(keyToPress))
        {
            theSR.sprite = defaultImage;
        }
    }
}
