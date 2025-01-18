using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializeGameObject : MonoBehaviour
{
    protected Vector3 moving;
    protected float speed;

    void Start()
    {
        this.Initialize();
    }

    protected virtual void Initialize()
    {
        this.moving = Vector3.zero;

        this.speed = 0f;
    }
}
