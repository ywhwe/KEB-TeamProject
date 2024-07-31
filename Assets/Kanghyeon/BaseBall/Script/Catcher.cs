using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    public BallManager ballmanager;
    public TextMeshProUGUI text;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==6)
        {
            BaseBallGameManager.instance.CountBall();
            Destroy(other.gameObject);
            Board.instance.hitFlag = false;
            StartCoroutine(Board.instance.TextScreenOn());
        }
    }
}
