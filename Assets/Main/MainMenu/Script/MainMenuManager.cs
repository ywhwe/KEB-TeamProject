using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;
    public Button scoreButton;
    
    void Start()
    {
        startButton.onClick.AddListener(TotalManager.instance.GoToIngame);
        startButton.onClick.AddListener(TotalManager.instance.ButtonSFX);
        exitButton.onClick.AddListener(TotalManager.instance.ExitGame);
        scoreButton.onClick.AddListener(TotalManager.instance.ScoreBoardTest);
    }
    
}
