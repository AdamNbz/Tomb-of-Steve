using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    enum Direction
    {
        North, South, East, West
    }

    public float speed;
    Rigidbody2D rb2d;
    Direction movingDir;
    public bool moveHorizontally, canCheck;
    public LayerMask obstacleMask;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (moveHorizontally)
        {
            if (Physics2D.Raycast(transform.position, Vector2.left, .6f, obstacleMask) || Physics2D.Raycast(transform.position, Vector2.right, .6f, obstacleMask))
            {
                canCheck = true;
            }
            else canCheck = false;
        }
        else
        {
            if (Physics2D.Raycast(transform.position, Vector2.up, .6f, obstacleMask) || Physics2D.Raycast(transform.position, Vector2.down, .6f, obstacleMask))
            {
                canCheck = true;
            }
            else canCheck = false;
        }

        if (canCheck)
        {
            if (Input.GetAxisRaw("Horizontal") != 0)
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionY;
                moveHorizontally = true;
                if (Input.GetAxisRaw("Horizontal") > 0)
                {
                    movingDir = Direction.East;
                }
                else
                {
                    movingDir = Direction.West;
                }
            }
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                rb2d.constraints = RigidbodyConstraints2D.FreezePositionX;
                moveHorizontally = false;
                if (Input.GetAxisRaw("Vertical") > 0)
                {
                    movingDir = Direction.North;
                }
                else
                {
                    movingDir = Direction.South;
                }
            }
        }
    }

    void FixedUpdate()
    {
        switch (movingDir)
        {
            case Direction.North:
                rb2d.linearVelocity = new Vector2(0, speed*Time.fixedDeltaTime);
                break;
            case Direction.South:
                rb2d.linearVelocity = new Vector2(0, -speed * Time.fixedDeltaTime);
                break;
            case Direction.East:
                rb2d.linearVelocity = new Vector2(speed * Time.fixedDeltaTime, 0);
                break;
            case Direction.West:
                rb2d.linearVelocity = new Vector2(-speed * Time.fixedDeltaTime, 0);
                break;
        }
    }
}
