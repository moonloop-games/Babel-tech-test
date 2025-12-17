using UnityEngine;

public class Jumper : MonoBehaviour
{
    public Rigidbody2D rb;

    public float jumpSpeed = 4;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public void Jump() => Jump(jumpSpeed);

    public void Jump(float speed)
    {
        rb.linearVelocityY += jumpSpeed;
    }
}
