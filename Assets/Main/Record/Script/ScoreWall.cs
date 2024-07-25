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
    public GameObject effect;

    private Vector3 posW;
    private Vector3 posA;
    private Vector3 posS;
    private Vector3 posD;

    private Vector3 poseffW;
    private Vector3 poseffA;
    private Vector3 poseffS;
    private Vector3 poseffD;

    private float hitangle = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        posW = boardForW.transform.position;
        posA = boardForA.transform.position;
        posD = boardForD.transform.position;
        posS = boardForS.transform.position;
        poseffW = new Vector3(2.45f, 0.6f, 0f);
        poseffA = new Vector3(2.9f, 0.6f, 0f);
        poseffS = new Vector3(3.37f, 0.6f, 0f);
        poseffD = new Vector3(3.8f, 0.6f, 0f);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
           JudgeNote(posW,poseffW);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            JudgeNote(posA,poseffA);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            JudgeNote(posS,poseffS);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            JudgeNote(posD,poseffD);
        }
    }

    private void JudgeNote(Vector3 Raycenter,Vector3 effpos)
    {
        if (Physics.BoxCast(Raycenter, new Vector3(0.1f, 0.1f, 0.15f), Vector3.down, out hitnote,
                Quaternion.identity,
                3f, 1 << 6))
        {
            if(Vector3.Angle(Vector3.right, hitnote.transform.position + Vector3.down * 0.08f) < hitangle)
                
                //if (Mathf.Abs(hitnote.transform.position.z) <= 0.2)
            {
                var vfx = Instantiate(hiteffect, effpos, Quaternion.identity);
                hitnote.collider.GetComponent<RecordNoteCon>().Destroynote();
                Destroy(vfx, 0.6f);
                RecordGameManager.instance.CountScoreRecord();
            }
            else
            {
                hitnote.collider.GetComponent<RecordNoteCon>().Blinknote();
            }
        }
        else
        {
            var vfx2 = Instantiate(effect, effpos, Quaternion.identity);
            Destroy(vfx2,0.2f);
        }
    }


}

