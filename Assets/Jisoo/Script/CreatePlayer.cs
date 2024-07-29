using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePlayer : MonoBehaviour
{
    public static CreatePlayer instance;
    public GameObject player1;

    private void Awake()
    {
        instance = this;
        GameObject temp = Instantiate(TotalManager.instance.playerPrefab, transform.position, transform.rotation);
        temp.transform.SetParent(transform);
        player1 = temp;
    }
}
