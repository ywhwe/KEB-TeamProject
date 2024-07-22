using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button customizeButton;
    public Button exitButton;
    public Image customizeScreen;
    public Button cusomizeScreenOffButton;

    void Start()
    {
        // startButton.onClick.AddListener(TotalManager.instance.GoToIngame);
        startButton.onClick.AddListener(() => SoundManager.instance.PlaySound("s1"));
        customizeButton.onClick.AddListener(CustomizeScreenOn);
        cusomizeScreenOffButton.onClick.AddListener(CustomizeScreenOff);
        exitButton.onClick.AddListener(TotalManager.instance.ExitGame);
        Instantiate(TotalManager.instance.playerPrefab);
    }
    
    public void CustomizeScreenOn()
    {
        customizeScreen.GameObject().SetActive(true);
    }
    
    public void CustomizeScreenOff()
    {
        customizeScreen.GameObject().SetActive(false);
    }
}
