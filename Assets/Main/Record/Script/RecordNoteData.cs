using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New RecordNote Data",menuName="CustomData/Create RecordNote Data")]
public class RecordNoteData : ScriptableObject
{
    public RecordNote[] notepos;
    
}
[Serializable]
public class RecordNote
{
    public List<float> sethaW;
    public List<float> sethaA;
    public List<float> sethaS;
    public List<float> sethaD;
    public float rotatespeed;
}
