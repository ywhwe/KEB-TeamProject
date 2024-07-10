using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManagerBtn : MonoBehaviour
{
    public GameObject railPrefab;
    public GameObject trolleyPrefab;
    [HideInInspector]
    public GameObject trolleyClone;

    public static GameManagerBtn instance;
    private Vector3 railPos = new (-12f, 0.05f, 0.5f);
    private Vector3 trolleyPos = new (-8f, 0.25f, -0.32f);
    private float railMove = 3f;

    private WaitForSeconds railDelay = new (0.5f);

    private void Awake()
    {
        StartCoroutine(railSet());
        trolleyClone = Instantiate(trolleyPrefab, trolleyPos, Quaternion.identity);
    }
    
    

    private IEnumerator railSet()
    {
        while (true)
        {
            yield return railDelay;

            railPos.x += railMove;
            railPos = new Vector3(railPos.x, railPos.y, railPos.z);
            Instantiate(railPrefab, railPos, Quaternion.identity);
        }
    }
}
