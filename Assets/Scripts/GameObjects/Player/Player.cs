using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Timeline;
using DG.Tweening;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public enum Movement
{
    Up,
    Left,
    Down,
    Right
}

public class PlayerScript : InitializeGameObject
{

    #region INITIALIZE COMPONENTS
    public PlayerVisual visual;

    private bool isPlaying, isMoving;

    public Transform raycastPosition1, raycastPosition2;

    private int currentAngle;

    private Queue<Movement> moveQueue;

    private Vector3 latestPosition;

    private const float raycast1Dist = 15.0f, raycast2Dist = 25.0f, raycast2PushOffset = -0.25f;

    #endregion

    #region FUNCTIONS

    protected override void Initialize()
    {
        this.moving = Vector3.zero;
        this.speed = 15f;

        this.moveQueue = new Queue<Movement>();

        this.isPlaying = false;
        this.isMoving = false;

        this.latestPosition = this.transform.position;

        this.currentAngle = 0;
    }

    private IEnumerator GameStartAnimation()
    {
        this.visual.PlayStartAnimation();

        yield return new WaitForSeconds(2.0f);

        this.isPlaying = true;

        this.visual.PlayAnimation(PlayerVisual.IDLE_ANIM);
    }


    public bool IsMoving()
    {
        return this.isMoving;
    }

    public void OnDead()
    {
        this.isPlaying = false;
        GameManager.Instance.OnPlayerDead();

        this.visual.PlayAnimation(PlayerVisual.DEATH_ANIM);
    }

    private void RotateZOnMoving(int angle)
    {
        this.currentAngle = angle;
        this.transform.DORotate(Vector3.forward * angle, 0f);
    }

    private void RotateZOnLanding()
    {
        this.currentAngle = (this.currentAngle + 180) % 360;
        this.transform.DORotate(Vector3.forward * this.currentAngle, 0f);
    }

    private void OnLanding()
    {
        this.RotateZOnLanding();
        this.visual.PlayAnimation(PlayerVisual.IDLE_ANIM);

        this.moving = Vector3.zero;
        this.isMoving = false;

        this.latestPosition = this.transform.position;
    }

    private bool ShootWallDetectRay(Transform raycastPos, out Vector3 movingDistance)
    {
        Vector2 rayOrigin = new Vector2(raycastPos.position.x, raycastPos.position.y);
        Vector2 rayDirection = new Vector2(this.moving.x, this.moving.y);
        float rayDistance = raycastPos == this.raycastPosition1 ? raycast1Dist * Time.deltaTime : 
                                                                  raycast2Dist * Time.deltaTime;
        int detectLayerMask = (1<<6);

        RaycastHit2D rayHit = Physics2D.Raycast(rayOrigin, rayDirection, rayDistance, detectLayerMask);

        if (raycastPos == this.raycastPosition1)
        {
            Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * rayDistance, Color.red);
        }
        else
        {
            Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * rayDistance, Color.blue);
        }


        if (rayHit.collider != null)
        {
            Vector3 collidedPoint = new Vector3(rayHit.point.x, rayHit.point.y, 0.0f);
            movingDistance = collidedPoint - new Vector3(rayOrigin.x, rayOrigin.y, 0.0f);
            movingDistance = movingDistance.magnitude >= 0.05f ? movingDistance : Vector3.zero;
            return true;
        }

        movingDistance = this.moving * this.speed * Time.deltaTime;
        return false;
    }

    private bool CheckWallCollision(out Vector3 movingDistance)
    {
        Vector3 movingDistance1;
        Vector3 movingDistance2;

        bool isHitOnRay1 = this.ShootWallDetectRay(this.raycastPosition1, out movingDistance1);
        bool isHitOnRay2 = this.ShootWallDetectRay(this.raycastPosition2, out movingDistance2);

        if (isHitOnRay2)
        {
            movingDistance = movingDistance2 + this.moving * raycast2PushOffset;
            return isHitOnRay2;
        }
        else
        {
            movingDistance = movingDistance1;
            return isHitOnRay1;
        }
    }

    private void Move(Vector3 movingDistance)
    {
        this.transform.position += movingDistance;
    }

    public void RegisterNextMove(Movement nextMove)
    {
        if (!this.isPlaying) return;
        this.moveQueue.Enqueue(nextMove);
    }

    private void PrepareForNextMove()
    {
        if (this.moveQueue.Count > 0)
        {
            Movement nextMove = this.moveQueue.Dequeue();

            switch (nextMove)
            {
                case Movement.Up:
                    this.moving = Vector3.up;
                    this.RotateZOnMoving(0);
                    break;
                case Movement.Left:
                    this.moving = Vector3.left;
                    this.RotateZOnMoving(90);
                    break;
                case Movement.Down:
                    this.moving = Vector3.down;
                    this.RotateZOnMoving(180);
                    break;
                case Movement.Right:
                    this.moving = Vector3.right;
                    this.RotateZOnMoving(270);
                    break;
                default:
                    break;
            }

            this.isMoving = true;
        }
    }

    private void AttemptToMove()
    {
        Vector3 movingDistance = Vector3.zero;

        bool isWallCollided = this.CheckWallCollision(out movingDistance);

        this.Move(movingDistance);

        if (!isWallCollided)
            this.visual.PlayAnimation(PlayerVisual.JUMP_ANIM);
        else
            this.OnLanding();
    }

    #endregion

    public void StartGame()
    {
        StartCoroutine(this.GameStartAnimation());
    }

    private void FixedUpdate()
    {
        if (this.isPlaying)
        {
            if (!this.isMoving)
                this.PrepareForNextMove();
            else
                this.AttemptToMove();
        }   
    }

}