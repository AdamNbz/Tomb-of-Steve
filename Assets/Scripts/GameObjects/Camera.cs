using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : InitializeGameObject
{
    private Transform playerTransform;

    private void FollowingPlayer()
    {
        Vector3 newPosition = this.playerTransform.position;
        
        newPosition.z = this.playerTransform.position.z;

        this.transform.DOMove(newPosition, 0.5f).SetId(this.GetInstanceID());
    }

    private void OnDisable()
    {
        DOTween.Kill(this.GetInstanceID());
    }

    private void Update()
    {
        if (this.playerTransform != null) this.FollowingPlayer();
    }

}
