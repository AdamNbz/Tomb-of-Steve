using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeVisual : MonoBehaviour
{
    [SerializeField] protected Animator objAnimator;
    [SerializeField] protected SpriteRenderer objSR;

#if UNITY_EDITOR
    
    protected virtual void Awake()
    {
        this.objAnimator = this.GetComponent<Animator>();
    }

#endif

}
