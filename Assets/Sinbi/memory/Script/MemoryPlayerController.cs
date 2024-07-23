using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryPlayerController : MonoBehaviour
{
    public Animator ani;
    public static MemoryPlayerController instance;
    
    protected static readonly int IsWMove = Animator.StringToHash("isWMove");
    protected static readonly int IsAMove = Animator.StringToHash("isAMove");
    protected static readonly int IsSMove = Animator.StringToHash("isSMove");
    protected static readonly int IsDMove = Animator.StringToHash("isDMove");

    public static float a = 5f; 
    public List<int> randomMotions = new List<int>();
    protected WaitForSeconds calTime = new WaitForSeconds(a);
    
    
    public GameObject player;
    protected List<int> playerMotions = new List<int>();
    public int playerLife = 2; 

    private void Awake() 
    {
        instance = this; 
    }

    void Start()
    {
        ani.GetComponent<Animator>();
    }
    
    void Update()
    {
        if (player.GetComponent<CharacterControl>().motionNumber != 0)
        {
            playerMotions.Add(player.GetComponent<CharacterControl>().motionNumber);
        }
    }

    public IEnumerator PlayGameRoutine()
    {
        RandomAction();
        
        yield return calTime;
        a=a+0.5f;
        
        Debug.Log("Player Motions: " + string.Join(", ", playerMotions));
        
        playerLife = CompareMotionNumber(playerMotions, randomMotions, playerLife);
        
        Debug.Log("Life: " + playerLife);
        
        
        
        if (playerLife == 0)
        {
            EndGame();
        }
        else
        {
            playerMotions.Clear();
            StartCoroutine(PlayGameRoutine());   
        }
    }
    
    public void RandomAction()
    {
        int randomMotionNumber = Random.Range(1, 5);
        randomMotions.Add(randomMotionNumber);

        foreach (int motion in randomMotions)
        {
            if (motion == 1)
            {
                ani.SetTrigger(IsWMove);
            }
            else if (motion == 2)
            {
                ani.SetTrigger(IsAMove);
            }
            else if (motion == 3)
            {
                ani.SetTrigger(IsSMove);
            }
            else if (motion == 4)
            {
                ani.SetTrigger(IsDMove);
            }
        }

        Debug.Log("Random Motions: " + string.Join(", ", randomMotions));
    }

    public int CompareMotionNumber(List<int> playerMotions, List<int> randomMotions, int playerLife)
    {
        if (playerMotions.Count != randomMotions.Count)
        {
            return playerLife - 1;
        }

        for (int i = 0; i < randomMotions.Count; i++)
        {
            if (playerMotions[i] != randomMotions[i])
            {
                return playerLife - 1;
            }
        }

        return playerLife;
    }

    public void StartGame()
    {
        StartCoroutine(PlayGameRoutine());
    }

    public void EndGame()
    {
        Time.timeScale = 1f;
        MemoryGameManager.instance.GameEnd();
    }
}
