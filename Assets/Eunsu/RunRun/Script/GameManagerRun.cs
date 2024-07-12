using System;
using UnityEngine;

public class GameManagerRun : WholeGameManager // Need fix for inheritance
{
    public static GameManagerRun instance;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        // When GameManager inheritance Fixed, this should be in StartGame()
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
            StopCoroutine(NoteController.instance.GenNotes());
            // UnityEditor.EditorApplication.ExitPlaymode();
        }
    }

    public override void GameStart()
    {
        StartCoroutine(NoteController.instance.GenNotes());
    }

    public override void GetScore()
    {
        var score = ScoreBoard.scoreInstance.score;
    }

    public override void GameEnd()
    {
        throw new NotImplementedException();
    }
}
