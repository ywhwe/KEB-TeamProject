using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoteController : MonoBehaviour
{
    private WaitForSeconds term = new(0.9f);
    
    private Transform canvasTrans;
    
    public GameObject UpNotePrefab;
    public GameObject DownNotePrefab;
    
    private GameObject upNote;
    private Vector3 upNotePos = new Vector3(910f, 75f, 0f);
    
    private GameObject downNote;
    private Vector3 downNotePos = new Vector3(910f, -75f, 0f);
    
    private float timeLimit = 0.2f;
    private float curTime;
    public float genRate;
    private Note note;
    private int rand;

    private bool isTimedOut;
    public bool IsTimedOut => isTimedOut;
    
    private bool isFinished;
    public bool IsFinished
    {
        get => isFinished;
        set => isFinished = value;
    }

    public int noteCount = 0;

    public static NoteController instance;

    private void Awake()
    {
        instance = this;
        canvasTrans = GameObject.FindGameObjectWithTag("Canvas").transform;
    }

    public IEnumerator GenNotes()
    {
        curTime = 0f;
        isTimedOut = false;
        isFinished = false;
        
        while (true)
        {
            yield return term;
            
            curTime += Time.deltaTime;
            
            rand = Random.Range(0, 101);

            if (curTime > timeLimit)
            {
                isTimedOut = true;
                break;
            }

            switch (rand)
            {
                case > 50 and <= 100:
                    upNote = Instantiate(UpNotePrefab, upNotePos, Quaternion.identity);
                    upNote.transform.SetParent(canvasTrans, false);
                    noteCount++;
                    break;
                case > 0 and <= 50:
                    downNote = Instantiate(DownNotePrefab, downNotePos, Quaternion.identity);
                    downNote.transform.SetParent(canvasTrans, false);
                    noteCount++;
                    break;
                default:
                    Debug.Log("Unexpected Range");
                    break;
            }
        }
    }

    public void GenStop()
    {
        StopCoroutine(GenNotes());
        Debug.Log("Gen Stopped");
    }
}
