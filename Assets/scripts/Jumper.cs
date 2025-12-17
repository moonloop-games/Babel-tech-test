using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Rigidbody2D rb;
    public float jumpSpeed = 12;

    private bool isGrounded = false;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }




    public void Jump() => Jump(jumpSpeed);

    public void Jump(float speed)
    {
        if (isGrounded)
        {
            rb.linearVelocityY += jumpSpeed;
        }
    }
}
