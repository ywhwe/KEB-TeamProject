using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    public void PushButton()
    {
        AudioSource pushButtonSFX = GetComponent<AudioSource>();
        pushButtonSFX.Play();
    }


}
