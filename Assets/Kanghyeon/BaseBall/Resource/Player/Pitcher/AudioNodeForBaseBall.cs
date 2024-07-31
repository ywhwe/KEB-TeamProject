using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioNodeForBaseBall : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    public void Play(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
        StartCoroutine(WaitSound());
    }


    private IEnumerator WaitSound()
    {
        yield return new WaitWhile(() => audioSource.isPlaying);
        
        SoundManagerForBaseBall.instance.SetNode(this);
    }
}
