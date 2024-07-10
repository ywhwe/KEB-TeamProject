using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Bat : MonoBehaviour
{
    private Animator ani;

    public BallManager ballmanager;

    public PitcherSquid pitchersquid;
    
    private static readonly int Swing = Animator.StringToHash("Swing");

    private RaycastHit ballhit;

    private float ballendtime;
    
    void Start()
    {
        
        ani = GetComponent<Animator>();
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ani.SetTrigger(Swing);
            BatRayCast();
            SwingBat();
            ballmanager.Check(ballhit.collider.GetComponent<TestBall>().endtime);
        }
    }

    public void SwingBat()
    {
        if (ballhit.collider != null)
        {
            var ballcomp = ballhit.collider.GetComponent<TestBall>();
            if (0<=(ballcomp.endtime-Time.time)&&(ballcomp.endtime-Time.time)<=0.3f)
            {
                var time = Mathf.Abs(2f - ballhit.transform.position.x) / ballcomp.ballspeed;
                Invoke("BallAway",time);
            }
        }
    }

    private void BallAway()
    {
        ballhit.collider.GetComponent<Rigidbody>().AddForce(-24f,10f,5f,ForceMode.Impulse);
    }

    private void BatRayCast()
    {
        var pos = transform.position;
        pos.x = 3f;
        Physics.BoxCast(pos, new Vector3(0.5f, 0.5f, 0.5f), Vector3.left, out ballhit,
            Quaternion.identity, 5f);
    }
    // public void Check(float endtime)
    // {
    //     
    //     {
    //         ballscore.text = "Finish";
    //         return;
    //     }
    //
    //     if (0<=(endtime-Time.time)&&(endtime-Time.time) <= 0.3f)
    //     {
    //         ballscore.text = "HomeRun";
    //         ballorder.RemoveAt(0);
    //         return;
    //     }
    //
    //     ballscore.text = "Strike";
    //     ballorder.RemoveAt(0);
    // }

}
