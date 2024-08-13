using System;
using UnityEngine;
using UnityEngine.UI;
using static System.MathF;
using Cysharp.Threading.Tasks;

public class CRTButtonController : MonoBehaviour
{
    private Image img;
    
    public Sprite defaultImage;
    public Sprite pressedImage;
    
    private Vector2 hitboxSize;

    [SerializeField] private GameObject hitFX;
    
    public KeyCode keyToPress;

    public int noteScore;

    private void Awake()
    {
        hitboxSize.x = 30f;
        hitboxSize.y = 100f;

        img = GetComponent<Image>();
        img.sprite = defaultImage;
    }

    private void Start()
    {
        InputControl().Forget();
    }

    private async UniTask InputControl()
    {
        while(!CRTGameManager.instance.isFinished)
        {
            await UniTask.Yield();
            
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
                            noteScore = 200;
                            break;
                        case < 15:
                            noteScore = 100;
                            break;
                        default:
                            noteScore = 50;
                            break;
                    }
                
                    SoundManagerForCRT.instance.PlaySound("ClearNote");
                
                    var fx = Instantiate(hitFX, judge.transform.position, Quaternion.identity, gameObject.transform);
                
                    Destroy(judge.gameObject);
                    Destroy(fx,0.5f);
                
                    CRTGameManager.instance.noteCount--;
                
                    CRTGameManager.instance.SetScore(noteScore);
                }
            }
        
            if (Input.GetKeyUp(keyToPress))
            {
                img.sprite = defaultImage;
            }
        }
    }
}
