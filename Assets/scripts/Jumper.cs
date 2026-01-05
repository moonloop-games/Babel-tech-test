using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask obstacleLayer;
    public Patrol patrol;

    [Header("Jump Settings")]
    public float jumpPower = 15;

    [Header("Jump Info")]
    float gravity => Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
    public float maxJumpHeight => jumpPower * jumpPower / (2f * gravity);

    public float maxJumpDistance => patrol.walkSpeed * timeInAir;

    public float timeInAir => 2f * jumpPower / gravity;

    [Header("Jump State")]
    public bool isGrounded = false;
    public bool isOnObstacle = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;

                    // check if we're on an obstacle or regular ground
                    if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
                    {
                        isOnObstacle = true;
                    }
                    else
                    {
                        isOnObstacle = false;
                    }

                    return;
                }
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            isOnObstacle = false;
        }
    }

    public void Jump() => Jump(jumpPower);

    public void Jump(float speed)
    {
        if (isGrounded)
        {
            rb.linearVelocityY = jumpPower;
        }
    }
}
