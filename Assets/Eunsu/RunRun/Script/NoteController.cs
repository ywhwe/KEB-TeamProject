using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;

public class NoteController : MonoBehaviour
{
    public float genRate;
    private float genTime;
    private Note note;
    
    public GameObject UpNotePrefab;
    public GameObject DownNotePrefab;
    
    private Transform canvasTrans;

    private GameObject upNote;
    private Vector3 upNotePos = new Vector3(910f, 75f, 0f);
    
    private GameObject downNote;
    private Vector3 downNotePos = new Vector3(910f, -75f, 0f);


    private void Awake()
    {
        canvasTrans = GameObject.FindGameObjectWithTag("Canvas").transform;
    }
    
    public void GenNotes(int dir)
    {
        genTime += Time.deltaTime;
        if (!(genTime > genRate)) return;

        genTime -= genRate;
        
        switch (dir)
        {
            case > 50 and <= 100:
                upNote = Instantiate(UpNotePrefab, upNotePos, Quaternion.identity);
                upNote.transform.SetParent(canvasTrans, false);
                break;
            case > 0 and <= 50:
                downNote = Instantiate(DownNotePrefab, downNotePos, Quaternion.identity);
                downNote.transform.SetParent(canvasTrans, false);
                break;
            default:
                Debug.Log("Unexpected Range");
                break;
        }
    }

    public void DestroyNotes(GameObject note)
    {
        Destroy(note);
        Debug.Log("note destroyed");
    }
}
