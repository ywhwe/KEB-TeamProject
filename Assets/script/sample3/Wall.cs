using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Wall : MonoBehaviour
{

    private Vector3 notepos;

    public TextMeshProUGUI scoreboard;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            transform.position=new Vector3(-0.969f, 0.01f, 0f);
            if (Mathf.Abs(notepos.z) < 0.1f)
            {
                scoreboard.text = "prefect";
            }
            if (Mathf.Abs(notepos.z) > 0.3f)
            {
                scoreboard.text = "miss";
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            transform.position=new Vector3(-0.371f, 0.01f, 0f);
            if (Mathf.Abs(notepos.z) < 0.1f)
            {
                scoreboard.text = "prefect";
            }
            if (Mathf.Abs(notepos.z) > 0.3f)
            {
                scoreboard.text = "miss";
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            transform.position=new Vector3(0.229f, 0.01f, 0f);
            if (Mathf.Abs(notepos.z) < 0.1f)
            {
                scoreboard.text = "prefect";
            }
            if (Mathf.Abs(notepos.z) > 0.3f)
            {
                scoreboard.text = "miss";
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 7)
        {
    
            notepos = other.transform.position;
            Debug.Log(other.transform.position);
        }
    }
}
