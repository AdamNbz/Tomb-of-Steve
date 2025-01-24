using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public enum TouchDirection
{
    Up = 0,
    Down = 1,
    Left = 2,
    Right = 3
}

public class GameInputManager : MonoSingleton<GameInputManager>
{

    #region INITIALIZE COMPONENTS
    public PlayerInput pi;

    private InputAction screenTouch, screenHold;

    public UnityAction<TouchDirection> onPlayerSwipe;

    private Vector3 startPosition = Vector3.negativeInfinity, endPosition = Vector3.negativeInfinity;

    private const double maxDuration = 1.0f, dotProductThreshold = 0.775f;
    #endregion

    #region FUNCTIONS

    private void InitializeInputAction()
    {
        this.screenHold = this.pi.actions["ScreenHold"];
        this.screenTouch = this.pi.actions["ScreenTouch"];
    }

    public void SetGameActive(bool check)
    {
        this.gameObject.SetActive(check);
    }

    private void DectectSwiping(double startTime, double time)
    {
        if (this.startPosition.Equals(Vector3.negativeInfinity)) return;

        if (this.endPosition.Equals(Vector3.negativeInfinity)) return;

        if (time - startTime <= maxDuration)
        {
            this.OnPlayerSwiped();
        }
    }

    private void OnPlayerSwiped()
    {
        Vector3 swipeDirection = (this.endPosition - this.startPosition).normalized;

        float dotUp = Vector3.Dot(Vector3.up, swipeDirection);
        float dotLeft = Vector3.Dot(Vector3.left, swipeDirection);
        float dotDown = Vector3.Dot(Vector3.down, swipeDirection);
        float dotRight = Vector3.Dot(Vector3.right, swipeDirection);

        float dotMax = Mathf.Max(dotUp, dotLeft, dotDown, dotRight);

        if (dotMax >= dotProductThreshold)
        {
            if (dotMax == dotUp)
            {
                this.InvokeOnPlayerSwipedCallback(TouchDirection.Up);
            }
            else if (dotMax == dotLeft)
            {
                this.InvokeOnPlayerSwipedCallback(TouchDirection.Left);
            }
            else if (dotMax == dotDown)
            {
                this.InvokeOnPlayerSwipedCallback(TouchDirection.Down);
            }
            else if (dotMax == dotRight)
            {
                this.InvokeOnPlayerSwipedCallback(TouchDirection.Right);
            }
        }
    }

    public void AssignOnPlayerSwipedCallback(UnityAction<TouchDirection> callback)
    {
        this.onPlayerSwipe -= callback;
        this.onPlayerSwipe += callback;
    }

    public void UnAssignOnPlayerSwipedCallback(UnityAction<TouchDirection> callback)
    {
        this.onPlayerSwipe -= callback;
    }

    public void InvokeOnPlayerSwipedCallback(TouchDirection swipeDirection)
    {
        this.onPlayerSwipe?.Invoke(swipeDirection);
    }

    private void OnScreenTouch(InputAction.CallbackContext ct)
    {
        float checkRelease = ct.ReadValue<float>();

        if (checkRelease == 0)
        {
            this.DectectSwiping(ct.startTime, ct.time);
            this.startPosition = Vector3.negativeInfinity;
            this.endPosition = Vector3.negativeInfinity;
        }   
    }

    private void OnScreenHold(InputAction.CallbackContext ct)
    {
        Vector2 currentPos = ct.ReadValue<Vector2>();

        if (currentPos.y > 1290) return;

        if (this.startPosition.Equals(Vector3.negativeInfinity))
            this.startPosition = new Vector3(currentPos.x, currentPos.y);
        else
            this.endPosition = new Vector3(currentPos.x, currentPos.y);
    }

    private void AssignInputEventListeners()
    {
        this.screenTouch.performed -= this.OnScreenTouch;
        this.screenTouch.performed += this.OnScreenTouch;

        this.screenHold.performed -= this.OnScreenHold;
        this.screenHold.performed += this.OnScreenHold;
    }

    private void UnAssignInputEventListeners()
    {
        this.screenTouch.performed -= this.OnScreenTouch;
        this.screenHold.performed -= this.OnScreenHold;
    }

    #endregion
    public void StartGame()
    {
        this.startPosition = Vector3.negativeInfinity;
        this.endPosition = Vector3.negativeInfinity;
    }

    private void OnEnable()
    {
        this.AssignInputEventListeners();
    }

    private void OnDisable()
    {
        this.UnAssignInputEventListeners();
    }

    private void Awake()
    {
        this.InitializeInputAction();
    }
}
