using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public NoteController controller;
    private int rand = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rand = Random.Range(0, 101);
        controller.GenNotes(rand);
        if (Time.deltaTime > 1f)
        {
            controller.DestroyNotes();
            Debug.Log("destroyed");
        }
    }
}
