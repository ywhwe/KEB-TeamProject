using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class WaterWave : MonoBehaviour 
{
    public GameObject water;

    private float xPos;
    private float zPos;
    private float yPos;
    public float waveSpeed = 1f;
    public float waveHight = 1f;
    void Start()
    {
        xPos = water.transform.position.x;
        yPos = water.transform.position.y;
        zPos = water.transform.position.z;
    }

    void Update()
    {
        float time = Time.time;
        water.transform.position = new Vector3(xPos, yPos+Mathf.Sin(time * waveSpeed) * waveHight, zPos);


    }
}