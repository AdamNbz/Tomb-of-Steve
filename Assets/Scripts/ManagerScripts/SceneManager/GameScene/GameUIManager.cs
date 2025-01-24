using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoSingleton<GameUIManager>
{
    #region INITIALIZE COMPONENTS

    public Transform canvasTransform;
    public CameraShaker cameraShaker;

    private const float shakeDuration = 0.5f;
    private const float shakeMagnitude = 0.5f;

    #endregion

    #region FUNCTIONS
    public void ShakeCamera()
    {
        StartCoroutine(this.cameraShaker.Shake(shakeDuration, shakeMagnitude));
    }

    public Transform GetCanvasPosition()
    {
        return this.canvasTransform;
    }
    #endregion

    public void StartGame()
    {

    }
}
