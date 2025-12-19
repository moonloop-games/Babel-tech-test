using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Rigidbody2D rb;
    public LayerMask obstacleLayer;
    public Patrol patrol;

    [Header("Jump Settings")]
    public float jumpPower = 15;

    [Header("Jump Info")]
    public float maxJumpHeight;
    public float maxJumpDistance;

    public float timeInAir;

    [Header("Jump State")]
    public bool isGrounded = false;
    private bool isOnObstacle = false;

    void Start()
    {
        CalculateJumpInfo();
    }

    private void CalculateJumpInfo()
    {
        // Physics formula: maxHeight = (velocity^2) / (2 * gravity)
        float gravity = Mathf.Abs(Physics2D.gravity.y * rb.gravityScale);
        maxJumpHeight = jumpPower * jumpPower / (2f * gravity);

        // Physics formula: timeInAir = 2 * velocity / gravity
        timeInAir = 2f * jumpPower / gravity;

        if (patrol != null)
        {
            maxJumpDistance = patrol.walkSpeed * timeInAir;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    isGrounded = true;

                    if (((1 << collision.gameObject.layer) & obstacleLayer) != 0)
                    {
                        isOnObstacle = true;
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
        if (isGrounded && !isOnObstacle)
        {
            rb.linearVelocityY = jumpPower;
        }
    }
}
