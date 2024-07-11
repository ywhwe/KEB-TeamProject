using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;
    
    public void SetLevel(float sliderVal) {
        mixer.SetFloat("BGM", Mathf.Log10(sliderVal)*20);
    }
}
