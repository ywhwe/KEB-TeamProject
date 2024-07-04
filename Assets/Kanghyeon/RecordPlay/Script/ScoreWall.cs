using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreWall : MonoBehaviour
{
    public GameObject boardForW;
    public GameObject boardForA;
    public GameObject boardForS;
    public GameObject boardForD;
    private Vector3 notepos;
    private RaycastHit hitnote;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Physics.BoxCast(boardForW.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down, out hitnote,
                    Quaternion.identity,
                    3f, 1 << 6))
            {
                Destroy(hitnote.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Physics.BoxCast(boardForA.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down, out hitnote,
                    Quaternion.identity,
                    3f, 1 << 6))
            {
                Destroy(hitnote.collider.gameObject);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {

            if (Physics.BoxCast(boardForS.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down,
                    out hitnote, Quaternion.identity,
                    3f, 1 << 6))
            {
                Destroy(hitnote.collider.gameObject);
            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Physics.BoxCast(boardForD.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down,
                    out hitnote, Quaternion.identity,
                    3f, 1 << 6))
            {
                Destroy(hitnote.collider.gameObject);
            }
        }
    }


}

