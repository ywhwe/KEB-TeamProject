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
    private readonly Vector3 upNotePos = new (910f, 100f, 0f);
    
    [Header("DownNote")]
    public GameObject DownNotePrefab;
    private GameObject downNote;
    private readonly Vector3 downNotePos = new (910f, -100f, 0f);
    
    private int rand;
    
    [HideInInspector] public int noteNumber;

    private const float musicBPM = 117f;
    private float musicTempo = 4f;
    private const float stdBPM = 60f;
    private float stdTempo = 4f;

    private float genTime = 0f;

    public bool IsTimedOut { get; private set; }

    public bool IsFinished { get; set; }

    public int noteCount = 0;

    private void Awake()
    {
        instance = this;
        canvasTrans = canvas.transform;
        
        IsTimedOut = false;
        IsFinished = false;
    }

    private void Update()
    {
        genTime = (stdBPM / musicBPM) * (musicTempo / stdTempo);
    }

    public IEnumerator GenNotes()
    {
        while (true)
        {
            yield return new WaitForSeconds(genTime);
            
            rand = Random.Range(0, 101);

            switch (rand)
            {
                case > 51 and <= 100:
                    upNote = Instantiate(UpNotePrefab, upNotePos, Quaternion.identity);
                    upNote.transform.SetParent(canvasTrans, false);
                    noteCount++;
                    noteNumber++;
                    break;
                
                case > 0 and <= 50:
                    downNote = Instantiate(DownNotePrefab, downNotePos, Quaternion.identity);
                    downNote.transform.SetParent(canvasTrans, false);
                    noteCount++;
                    noteNumber++;
                    break;
                
                default:
                    Debug.Log("Unexpected Range");
                    break;
            }

            if (noteNumber <= 115) continue;
            IsTimedOut = true;
            break;
        }
    }
}
