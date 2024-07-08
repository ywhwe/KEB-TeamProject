using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public static BallSpawner instance;
    public BaseBallData[] balldb;
    public List<BallData> balllist;

    public GameObject fastball;
    public GameObject normalball;
    public static float endtime;
    private float shoottime;

    public Transform battransform;
    
    private RaycastHit hitball;

    public TextMeshProUGUI ballscore;
    // Start is called before the first frame update
    void Start()
    {
        balllist = new List<BallData>();
        foreach (var data in balldb[0].balldata)
        {
            balllist.Add(data);
        }

        ShootBall();


    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log(Time.time);
    }

    private void ShootBall()
    {
        shoottime = Time.time;
        endtime = shoottime + balllist[0].delay;
        ThrowBall();
        if (balllist.Count > 1)
        {
            Invoke("ShootBall",balllist[0].delay);
            balllist.RemoveAt(0);
        }
        
    }

     private void ThrowBall()
     { 
         var speed = balllist[0].ballspeed;
         var ball = Instantiate(normalball, transform.position, Quaternion.identity);
         ball.GetComponent<Rigidbody>().AddForce(new Vector3(1f*speed,0f,0f),ForceMode.VelocityChange);
         Destroy(ball,8f);
     }
     public void Check()
     {
         Physics.Raycast(battransform.position, Vector3.left, out hitball,
             5f);
         var ball = hitball.collider.gameObject;
         if (Mathf.Abs(Time.time - endtime) < 0.5f)
         {
             ball.GetComponent<Rigidbody>().AddForce(-15f,15f,5f,ForceMode.Impulse);
             ballscore.text = "HomeRun";
         }
         else
         {
             ballscore.text = "Strike";
         }
     }
}
