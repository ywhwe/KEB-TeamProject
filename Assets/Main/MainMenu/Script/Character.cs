using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject[] characterPrefab;
    public GameObject currentCharacter;
    void Start()
    {
        currentCharacter = Instantiate(characterPrefab[0],this.transform.position,this.transform.rotation);
        currentCharacter.transform.parent = this.transform;
        currentCharacter.transform.localScale = new Vector3(1f, 1f, 1f);
    }

    void Update()
    {
        
    }
}
