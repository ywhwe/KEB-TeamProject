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

    public GameObject hiteffect;

    private Vector3 posW;
    private Vector3 posA;
    private Vector3 posS;
    private Vector3 posD;
    // Start is called before the first frame update
    void Start()
    {
        posW = boardForW.transform.position;
        posA = boardForA.transform.position;
        posD = boardForD.transform.position;
        posS = boardForS.transform.position;
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
                var vfx = Instantiate(hiteffect, new Vector3(2.4f,0.6f,0f), Quaternion.identity);
                Destroy(hitnote.collider.gameObject);
                Destroy(vfx,0.7f);
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Physics.BoxCast(boardForA.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down, out hitnote,
                    Quaternion.identity,
                    3f, 1 << 6))
            {
                var vfx = Instantiate(hiteffect, new Vector3(2.83f,0.6f,0f), Quaternion.identity);
                Destroy(hitnote.collider.gameObject);
                Destroy(vfx,0.7f);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {

            if (Physics.BoxCast(boardForS.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down,
                    out hitnote, Quaternion.identity,
                    3f, 1 << 6))
            {
                var vfx = Instantiate(hiteffect, new Vector3(3.3f,0.6f,0f), Quaternion.identity);
                Destroy(hitnote.collider.gameObject);
                Destroy(vfx,0.7f);
            }

        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Physics.BoxCast(boardForD.transform.position, new Vector3(0.1f, 0.1f, 0.1f), Vector3.down,
                    out hitnote, Quaternion.identity,
                    3f, 1 << 6))
            {
                var vfx = Instantiate(hiteffect, new Vector3(3.8f, 0.6f, 0f), Quaternion.identity);
                Destroy(hitnote.collider.gameObject);
                Destroy(vfx,0.7f);
            }
        }
    }


}

