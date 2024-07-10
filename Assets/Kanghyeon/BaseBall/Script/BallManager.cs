using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public BaseBallData[] balldb;
    private List<BallData> balllist;
    private List<float> ballorder;
    
    public TextMeshProUGUI ballscore;

    private static readonly int Curve = Animator.StringToHash("curve");
    private static readonly int HitTrigger = Animator.StringToHash("HitTrigger");
    
    void Start()
    {
        balllist = new List<BallData>();
        foreach (var data in balldb[0].balldata)
        {
            balllist.Add(data);
        }

        var templist = new List<float>();
        foreach (var var in balllist)
        {
            templist.Add(var.endtime);
        }

        templist.Sort();
        ballorder = templist;

    }
    
     public void Check(float endtime)
     {
         if (!ballorder.Any())
         {
             ballscore.text = "Finish";
             return;
         }

         if (0<=(endtime-Time.time)&&(endtime-Time.time) <= 0.3f)
         {
             ballscore.text = "HomeRun";
             ballorder.RemoveAt(0);
             return;
         }

         ballscore.text = "Strike";
         ballorder.RemoveAt(0);
     }

     public void DeleteOrder()
     {
         ballorder.RemoveAt(0);
     }
}
