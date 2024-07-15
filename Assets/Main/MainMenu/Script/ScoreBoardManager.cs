using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreBoardManager : MonoBehaviour
{
    public void RestartGame()
    {
        TotalManager.instance.GoToIngame();
    }

    public void GoToMainMenu()
    {
        TotalManager.instance.MoveScene(0);
    }
}
