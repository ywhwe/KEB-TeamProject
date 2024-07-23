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
    }

    void Start()
    {
        player1 = TotalManager.instance.playerPrefab;
        GameObject temp = Instantiate(player1, transform.position, Quaternion.identity);
        temp.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
