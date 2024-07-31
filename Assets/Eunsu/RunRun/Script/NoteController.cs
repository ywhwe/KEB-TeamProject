using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class NoteController : MonoBehaviour
{
    public static NoteController instance;
    
    public GameObject canvas;
    private Transform canvasTrans;
    
    [Header("UpNote")]
    public GameObject UpNotePrefab;
    private GameObject upNote;
    private Vector3 upNotePos = new Vector3(910f, 100f, 0f);
    
    [Header("DownNote")]
    public GameObject DownNotePrefab;
    private GameObject downNote;
    private Vector3 downNotePos = new Vector3(910f, -100f, 0f);
    
    private int rand, noteNumber;

    private float musicBPM = 117f;
    private float musicTempo = 4f;
    private float stdBPM = 60f;
    private float stdTempo = 4f;

    private float genTime = 0f;

    private bool isTimedOut;
    public bool IsTimedOut => isTimedOut;
    
    private bool isFinished;
    public bool IsFinished
    {
        get => isFinished;
        set => isFinished = value;
    }

    public int noteCount = 0;

    private void Awake()
    {
        instance = this;
        canvasTrans = canvas.transform;
    }

    private void Update()
    {
        genTime = (stdBPM / musicBPM) * (musicTempo / stdTempo);
    }

    public IEnumerator GenNotes()
    {
        isTimedOut = false;
        isFinished = false;
        
        while (true)
        {
            yield return new WaitForSeconds(genTime);
            
            noteNumber++;
            
            rand = Random.Range(0, 101);

            if (noteNumber > 115)
            {
                isTimedOut = true;
                break;
            }

            switch (rand)
            {
                case > 51 and <= 100:
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
}
