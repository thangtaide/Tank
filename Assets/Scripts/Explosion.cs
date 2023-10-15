using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private void Start()
    {
        SoundController.instance.PlaySound("explosion_sound");
    }
    public void OnAnimationComplete()
    {
        Destroy(gameObject);
    }
}
