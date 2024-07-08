using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoteSpawner : MonoBehaviour
{
    public GameObject noteW;
    public GameObject noteA;
    public GameObject noteS;
    public GameObject noteD;
    public Transform noteparents;
    public Records record;


    public float nowangle=0f;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            SpawnNote1();
            SpawnNote2();
            SpawnNote3();
            SpawnNote4();
        }

    }

    // Update is called once per frame
    void Update()
    {
        nowangle += Time.deltaTime * record.rotatespeed;
        if (nowangle >= 360)
        {
            nowangle -= 360;
        }
    }

 

    private void SpawnNote1()
    {
        var setha = Random.Range(0f, 360) * (Mathf.PI / 180);
        var position = new Vector3(Mathf.Cos(setha)*2.2f,0.08f,Mathf.Sin(setha)*2.2f);
        var newnote = Instantiate(noteW, position, Quaternion.identity);
        newnote.transform.SetParent(noteparents);

    }
    private void SpawnNote2()
    {
        var setha = Random.Range(0f, 360) * (Mathf.PI / 180);
        var position = new Vector3(Mathf.Cos(setha)*2.7f,0.08f,Mathf.Sin(setha)*2.7f);
        var newnote = Instantiate(noteA, position, Quaternion.identity);
        newnote.transform.SetParent(noteparents);

    }
    private void SpawnNote3()
    {
        var setha = Random.Range(0f, 360) * (Mathf.PI / 180);
        var position = new Vector3(Mathf.Cos(setha)*3.2f,0.08f,Mathf.Sin(setha)*3.2f);
        var newnote = Instantiate(noteS, position, Quaternion.identity);
        newnote.transform.SetParent(noteparents);

    }
    private void SpawnNote4()
    {
        var setha = Random.Range(0f, 360) * (Mathf.PI / 180);
        var position = new Vector3(Mathf.Cos(setha)*3.71f,0.08f,Mathf.Sin(setha)*3.71f);
        var newnote = Instantiate(noteD, position, Quaternion.identity);
        newnote.transform.SetParent(noteparents);

    }
}