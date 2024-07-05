using System;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(NoteController.instance.GenNotes());
        if (NoteController.instance.IsTimedOut) NoteController.instance.GenStop();
    }

    private void Update()
    {
        if (!(Time.deltaTime > 1f)) return;
        Destroy(NoteController.instance.gameObject);

        if (NoteController.instance.noteCount < 1)
        {
            NoteController.instance.IsFinished = true;
            Debug.Log("Note Cleared");
            UnityEditor.EditorApplication.ExitPlaymode();
        }
    }
    
    private void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else 
        Application.Quit();
#endif
    }
}
