using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Torch : MonoBehaviour
{
    public Animator torchAnimator;

    public float counter = 0.0f;

    private bool check = false;
    
    private const string FADE_ANIM = "Torch_Fading";

    private void Update()
    {
        if (this.check) return;

        this.counter += Time.deltaTime;

        if (this.counter >= 1.05f)
        {
            this.torchAnimator.Play(FADE_ANIM);
        }
    }
}
