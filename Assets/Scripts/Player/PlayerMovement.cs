using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{
    private enum MovementStatus
    {
        MOVING,
        IDLE
    }

    private MovementStatus currentMovementStatus = MovementStatus.IDLE;

    [Header("Movement Settings")]
    public float speed = 20f;
    public LayerMask obstacleMask;

    private Rigidbody2D rb2d;
    private Vector2 moveDirection;
    private bool isMoving;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.gravityScale = 0f;
    }

    void Update()
    {
        if (isMoving) return;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection = Vector2.up;
            StartMovement();
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection = Vector2.down;
            StartMovement();
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection = Vector2.left;
            StartMovement();
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection = Vector2.right;
            StartMovement();
        }
    }

    private void StartMovement()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDirection, Mathf.Infinity, obstacleMask);

        Vector2 targetPosition;
        if (hit.collider != null)
        {
            float distance = hit.distance - 0.5f;
            targetPosition = (Vector2)transform.position + moveDirection * distance;
        }
        else
        {
            targetPosition = (Vector2)transform.position + moveDirection * 50f;
        }

        targetPosition.x = Mathf.Round(targetPosition.x * 2f) / 2f;
        targetPosition.y = Mathf.Round(targetPosition.y * 2f) / 2f;

        StartCoroutine(MoveOverTime(transform.position, targetPosition));
    }

    private System.Collections.IEnumerator MoveOverTime(Vector2 start, Vector2 end)
    {
        isMoving = true;

        float distance = Vector2.Distance(start, end);
        float duration = distance / speed;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            rb2d.MovePosition(Vector2.Lerp(start, end, t));
            yield return null;
        }
        rb2d.MovePosition(end);
        isMoving = false;
    }
}
