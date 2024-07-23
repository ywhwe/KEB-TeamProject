using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour
{
    private int prevIndex = 0;
    
    
    public GameObject[] characterPrefab;

    void Start()
    {
        
    }

    public void CustomizeSelect(int prefabNumber)
    {
        characterPrefab[prevIndex].SetActive(false);
        characterPrefab[prefabNumber].SetActive(true);
        prevIndex = prefabNumber;
        TotalManager.instance.playerPrefabNumber = prefabNumber;
    }
}
