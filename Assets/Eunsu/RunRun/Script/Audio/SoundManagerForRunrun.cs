using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerForRunrun : MonoBehaviour
{
    public static SoundManagerForRunrun instance;
    
    public RunAudioData[] soundResources;
    private Dictionary<string, AudioClip> soundDB = new();

    public int poolSize;
    public GameObject soundNodePrefab;
    private Queue<AudioNodeForRunrun> soundPool = new();
    
    private void Awake()
    {
        instance = this;
        
        foreach (var soundResource in soundResources)
        {
            soundDB.Add(soundResource.key, soundResource.Clip); 
        }
        
        

        for (var i = 0; i < poolSize; i++)
        {
            MakeNode();
        }
    }

    private void MakeNode()
    {
        var audioNode = Instantiate(soundNodePrefab, transform).GetComponent<AudioNodeForRunrun>();
        soundPool.Enqueue(audioNode);
    }

    public void PlaySound(string key)
    {
        if (!soundDB.ContainsKey(key))
        {
            Debug.LogError("There is no key in sound DB: " + key);
            return;
        }
        
        var node = GetNode();
        
        node.transform.position = Vector3.zero;
        
        node.Play(soundDB[key]);
    }
    
    public void PlaySound(string key, Vector3 pos)
    {
        if (!soundDB.ContainsKey(key))
        {
            Debug.LogError("There is no key in sound DB: " + key);
            return;
        }
        
        var node = GetNode();
        
        node.transform.position = pos;
        
        node.Play(soundDB[key]);
    }

    public void PlaySound(string key, Transform parent)
    {
        if (!soundDB.ContainsKey(key))
        {
            Debug.LogError("There is no key in sound DB: " + key);
            return;
        }
        
        var node = GetNode();
        
        node.transform.SetParent(parent);
        node.transform.localPosition = Vector3.zero;
        
        node.Play(soundDB[key]);
    }
    
    private AudioNodeForRunrun GetNode()
    {
        if (soundPool.Count < 1)
        {
            MakeNode();
        }

        var node = soundPool.Dequeue();
        //
        return node;
    }

    public void SetNode(AudioNodeForRunrun node)
    {
        node.transform.SetParent(transform);
        
        soundPool.Enqueue(node);
    }
    
}

[Serializable]
public class RunAudioData
{
    public string key;
    public AudioClip Clip;
}