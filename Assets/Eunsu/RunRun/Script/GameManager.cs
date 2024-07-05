using System;
using UnityEngine;
using static NoteController;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(instance.GenNotes());
    }

    private void Update()
    {
        if (!(Time.deltaTime > 1f)) return;
        Destroy(instance.gameObject);
    }
    
    private void LateUpdate()
    {
        if (instance.noteCount < 1 && instance.IsTimedOut)
            instance.IsFinished = true;
        
        if (instance.IsFinished)
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
