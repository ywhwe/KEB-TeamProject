using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitJudgement : MonoBehaviour
{
    private bool _isHit = false;
    public bool isHit => _isHit;

    public void JudgeHit()
    {
        _isHit = true;
    }
}
