using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HpController : ProcessingController
{
    
    public float HP
    {
        get { return maxValue; }
        set
        {
            maxValue = value;
            CurrentValue = maxValue;
        }
    }
    public Action onDie;
    public void TakeDamage(float damage)
    {
        CurrentValue -= damage;
    }
    protected override void OnChangeValue(float value)
    {
        if(value == 0)
        {
            if(onDie != null)
            {
                onDie();
            }
        }
    }
}
