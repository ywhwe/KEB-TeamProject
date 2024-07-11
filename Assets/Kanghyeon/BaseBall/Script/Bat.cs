using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public TextMeshProUGUI scoreboard;
    
    void Start()
    {
        
        ani = GetComponent<Animator>();
        
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ani.SetTrigger(Swing);
            if (IsBallHit())
            {
                SwingBat();
                Check(ballendtime);
            }
            else
            {
                scoreboard.text = "Strike";
            }
            
            
        }
    }
    
    public void SwingBat()
    {
        if (0 <= (ballendtime - Time.time) && (ballendtime - Time.time) <= 0.3f)
        {
            var time = Mathf.Abs(2f - ballhit.transform.position.x) / ballhit.collider.GetComponent<TestBall>().ballspeed;
            Destroy(ballhit.collider.gameObject,3f);
            Invoke("BallAway", time);
            
        }
    }

    private void BallAway()
    {
        ballhit.collider.GetComponent<Rigidbody>().AddForce(-24f,10f,5f,ForceMode.Impulse);
    }
    
    private bool IsBallHit()
    {
        var pos = transform.position;
        pos.x = 3f;
        if (Physics.BoxCast(pos, new Vector3(0.5f, 0.5f, 0.5f), Vector3.left, out ballhit,
                Quaternion.identity, 5f))
        {
            ballendtime = ballhit.collider.GetComponent<TestBall>().endtime;
            return true;
        }

        return false;
    }
    
    public void Check(float endtime)
    {
        
        if (0<=(endtime-Time.time)&&(endtime-Time.time) <= 0.3f)
        {
            scoreboard.text = "HomeRun";
            ballmanager.CountScore();
            return;
        }
        scoreboard.text = "Strike";
    }

}
