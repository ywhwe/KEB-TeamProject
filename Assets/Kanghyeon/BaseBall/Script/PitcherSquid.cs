using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherSquid : MonoBehaviour
{
    public BaseBallData[] balldb;
    private List<BallData> balllist;
    private List<GameObject> ballobjlist;
    public GameObject Ball;

    private Animator ani;
    
    private static readonly int Curve = Animator.StringToHash("curve");
    private static readonly int ThrowAction = Animator.StringToHash("ThrowAction");
    private static readonly int HitTrigger = Animator.StringToHash("HitTrigger");

    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponent<Animator>();
       
        
    }

    public void StartCycle(int index)
    {
        CheckBalldb(index);
        ShootBall();
    }
    private void CheckBalldb(int index)
    {
        balllist = new List<BallData>();
        foreach (var data in balldb[0].balldatadbs[index].balldata)
        {
            balllist.Add(data);
        }
    }
    private void ThrowNum0Ball()
    {
 
        var ball = Instantiate(Ball, transform.position, Quaternion.identity);
        ball.GetComponent<Animator>().SetInteger(Curve,0);
        
    }
    
    private void ShootBall()
    {
        ani.SetTrigger(balllist[0].ballname);
        if (balllist.Count > 1)
        {
            Invoke("ShootBall",balllist[0].delay);
            
        }
        
    }

    public void TestBall()
    {
        var pos = transform.position;
        pos.x = -19f;
        pos.y = 1.4f;
        var ball = Instantiate(Ball, pos, Quaternion.identity).GetComponent<TestBall>();
        ball.Init(balllist[0].endtime+Time.time);
        balllist.RemoveAt(0);
    }

    public void SoundYap()
    {
        SoundManagerForBaseBall.instance.PlaySound("Yap");

    }

    public void SoundHap()
    {
        SoundManagerForBaseBall.instance.PlaySound("Hap");

    }
}
