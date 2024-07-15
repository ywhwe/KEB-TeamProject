using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class MainMenuManager : MonoBehaviour
{
    public Button startButton;
    public Button exitButton;

    void Start()
    {
        startButton.onClick.AddListener(TotalManager.instance.GoToIngame);
        startButton.onClick.AddListener(() => SoundManager.instance.PlaySound("s1"));
        exitButton.onClick.AddListener(TotalManager.instance.ExitGame);
    }

}
