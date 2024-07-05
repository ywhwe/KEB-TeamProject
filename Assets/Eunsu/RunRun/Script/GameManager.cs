using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(NoteController.instance.GenNotes());
    }

    private void Update()
    {
        if (!(Time.deltaTime > 1f)) return;
        Destroy(NoteController.instance.gameObject);
    }
    
    private void LateUpdate()
    {
        if (NoteController.instance.noteCount < 1 && NoteController.instance.IsTimedOut)
            NoteController.instance.IsFinished = true;
        
        if (NoteController.instance.IsFinished)
        {
            Debug.Log("Note Cleared");
            UnityEditor.EditorApplication.ExitPlaymode();
        }
    }
    
//     private void ExitGame()
//     {
// #if UNITY_EDITOR
//         UnityEditor.EditorApplication.isPlaying = false;
// #else 
//         Application.Quit();
// #endif
//     }
}
