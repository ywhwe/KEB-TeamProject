using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RecordNoteCon : MonoBehaviour
{
    private BoxCollider collider;
    public Material[] notemat;
    private MeshRenderer headmesh;

    async UniTaskVoid Disablenote()
    {
        collider.enabled = false;
        for (int i = 0; i < 4; i++)
        {
            headmesh.material = notemat[1];
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));
            headmesh.material = notemat[0];
            await UniTask.Delay(TimeSpan.FromSeconds(0.2f));

        }

        collider.enabled = true;
    }

    public void Blinknote()
    {
        Disablenote().Forget();
    }

    public void Destroynote()
    {
        Destroy(gameObject);
    }
    void Start()
    {
        collider = GetComponent<BoxCollider>();
        headmesh = transform.GetChild(1).GetComponent<MeshRenderer>();

    }
    
}
