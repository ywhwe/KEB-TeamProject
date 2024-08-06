using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Bat : MonoBehaviour
{
    private Animator ani;

    public PitcherSquid pitchersquid;

    private static readonly int Swing = Animator.StringToHash("Swing");

    private RaycastHit ballhit;

    private float ballendtime;

    public TextMeshProUGUI scoreboard;
    private bool isStart = true;
    public bool _isStart => isStart;


    void Start()
    {

        ani = GetComponent<Animator>();

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ani.SetTrigger(Swing);
            if (isStart)
            {
                if (IsBallHit())
                {
                    AwayBall();
                    Debug.Log(ballendtime);
                    Debug.Log(Time.time);
                 
                }
                else
                {
                    SoundSwing();
                }
            }
        }
    }

    private bool IsBallHit()
    {
        var pos = transform.position;
        pos.x = 3f;
        if (Physics.BoxCast(pos, new Vector3(0.3f, 0.3f, 0.3f), Vector3.left, out ballhit,
                Quaternion.identity, 5.5f))
        {
            ballendtime = ballhit.collider.GetComponent<TestBall>().endtime;
            return true;
        }

        return false;
    }

    public void AwayBall()
    {
        if (0 <= (ballendtime - Time.time) && (ballendtime - Time.time) <= 0.5f)
        {
            // var time = Mathf.Abs(2f - ballhit.transform.position.x) /
            //            ballhit.collider.GetComponent<TestBall>().ballspeed;
            Destroy(ballhit.collider.gameObject, 8f);
            Invoke("BallAway", 0.07f);
            Board.instance.OXImageOn(0);
            SoundManagerForBaseBall.instance.PlaySound("Homerun");
            BaseBallGameManager.instance.CountScore();
            SoundHit();

        }
        else
        {
            SoundSwing();
        }
    }

    private void BallAway()
    {
        var angle = new Vector3(-18f,7.4f,5f).normalized * 100f;
        ballhit.collider.GetComponent<Rigidbody>().AddForce(angle, ForceMode.VelocityChange);
        ballhit.collider.GetComponent<Rigidbody>().useGravity = true;
    }


    private void SoundHit()
    {
        SoundManagerForBaseBall.instance.PlaySound("Hit");

    }

    private void SoundSwing()
    {
        SoundManagerForBaseBall.instance.PlaySound("Swing");
    }

    public void IsGameStart()
    {
        isStart = true;
    }

    public void IsGameEnd()
    {
        isStart = false;
    }
}
