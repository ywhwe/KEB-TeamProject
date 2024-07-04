using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MainMenu : MonoBehaviour
{
    public Button gameStartButton;
    public Button gameExitButton;

    public void GoToIngame()
    {
        int gameNumber = Random.Range(1, 3);
        TotalManager.instance.MoveScene(gameNumber);
        gameStartButton.interactable = false;
    }
    
    public void ExitGame()
    {
#if  UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
