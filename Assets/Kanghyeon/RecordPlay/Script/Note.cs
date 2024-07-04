using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Note Data",menuName="CustomNote/Create Item Note")]
public class Note : MonoBehaviour
{
    public float noterang;
    
    public void Initnote(float rang)
    {
        noterang = rang;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
