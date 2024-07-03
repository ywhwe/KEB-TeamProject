using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public float genRate;
    private float genTime;
    
    public GameObject UpNotePrefab;
    public GameObject DownNotePrefab;
    
    private Transform canvasTrans;

    private GameObject upNote;
    private Vector3 upNotePos;
    
    private GameObject downNote;
    private Vector3 downNotePos;


    private void Awake()
    {
        canvasTrans = GameObject.FindGameObjectWithTag("Canvas").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void GenNotes(int dir)
    {
        genTime += Time.deltaTime;
        if (!(genTime > genRate)) return;

        genTime -= genRate;
        
        switch (dir)
        {
            case > 50 and <= 100:
                upNote = Instantiate(UpNotePrefab, new Vector3(910f, 75f, 0f), Quaternion.identity);
                upNote.transform.SetParent(canvasTrans, false);
                Debug.Log("gen up");
                break;
            case > 0 and <= 50:
                downNote = Instantiate(DownNotePrefab, new Vector3(910f, -75f, 0f), Quaternion.identity);
                downNote.transform.SetParent(canvasTrans, false);
                Debug.Log("gen down");
                break;
            default:
                Debug.Log("Unexpected Range");
                break;
        }
    }

    public void DestroyNotes()
    {
        Destroy(gameObject);
    }
}
