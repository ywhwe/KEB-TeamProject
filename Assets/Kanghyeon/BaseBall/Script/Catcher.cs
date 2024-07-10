using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Catcher : MonoBehaviour
{
    public BallManager _ballmanager;
    public TextMeshProUGUI text;
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer==6)
        {
            Destroy(other);
            _ballmanager.DeleteOrder();
            text.text = "Strike";
        }
    }
}
