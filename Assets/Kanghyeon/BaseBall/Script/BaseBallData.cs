using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "New Ball Data",menuName="CustomData/Create Ball Data")]
public class BaseBallData : ScriptableObject
{
    public BallData[] balldata;
}