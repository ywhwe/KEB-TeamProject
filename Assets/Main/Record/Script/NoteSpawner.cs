using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoteSpawner : MonoBehaviour
{
    public RecordNoteData notedb;
    
    public GameObject noteW;
    public GameObject noteA;
    public GameObject noteS;
    public GameObject noteD;
    public Transform noteparents;
    
    // Start is called before the first frame update
    // void Start()
    // {
    //     var randnum = Random.Range(0, 3);
    //     randnum = 0;
    //     foreach (var ang in notedb.notepos[randnum].sethaW)
    //     {
    //         SpawnNote(ang,2.23f,noteW);
    //     }
    //     foreach (var ang in notedb.notepos[randnum].sethaA)
    //     {
    //         SpawnNote(ang,2.75f,noteA);
    //     }
    //     foreach (var ang in notedb.notepos[randnum].sethaS)
    //     {
    //         SpawnNote(ang,3.25f,noteS);
    //     }
    //     foreach (var ang in notedb.notepos[randnum].sethaD)
    //     {
    //         SpawnNote(ang,3.71f,noteD);
    //     }
    //     
    // }
    
    public void RecordingNote(int randnum)
    {
        foreach (var ang in notedb.notepos[randnum].sethaW)
        {
            SpawnNote(ang,2.23f,noteW);
        }
        foreach (var ang in notedb.notepos[randnum].sethaA)
        {
            SpawnNote(ang,2.75f,noteA);
        }
        foreach (var ang in notedb.notepos[randnum].sethaS)
        {
            SpawnNote(ang,3.25f,noteS);
        }
        foreach (var ang in notedb.notepos[randnum].sethaD)
        {
            SpawnNote(ang,3.71f,noteD);
        }
    }
    
    
    private void SpawnNote(float angle,float rad,GameObject note)
    {
        var setha = angle * (Mathf.PI / 180);
        var position = new Vector3(Mathf.Cos(setha)*rad,0.08f,Mathf.Sin(setha)*rad);
        var newnote = Instantiate(note, position, Quaternion.Euler(0f, -angle, 0f));
        
        newnote.transform.SetParent(noteparents);
        RecordGameManager.instance.PlusCountRecord();

    }
   
}