using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinnerPodium : MonoBehaviour
{
    public GameObject winnerpos;

    public TextMeshProUGUI winnername;
    // Start is called before the first frame update
    void Start()
    {
        TotalManager.instance.gameRound = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
