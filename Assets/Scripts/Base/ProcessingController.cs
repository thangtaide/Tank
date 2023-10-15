using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProcessingController : MonoBehaviour
{
   
   public float currentValue;
    public float maxValue;
    public float CurrentValue { get { return currentValue; } 
        set
        {
            currentValue = value;
            if(currentValue<0) currentValue = 0;
            OnChangeValue(currentValue);
            DisplayValue();
        } 
    }
    protected abstract void OnChangeValue(float value);
    public void DisplayValue()
    {
        transform.localScale = new Vector3 ((float)currentValue /maxValue, transform.localScale.y, transform.localScale.z);
    }
}
