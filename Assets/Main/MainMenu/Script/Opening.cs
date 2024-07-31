using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    private void Start()
    {
        TotalManager.instance.MoveScene("Main");
    }
}
