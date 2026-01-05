using UnityEngine;
using UnityEngine.Events;

public class Patrol : MonoBehaviour
{
    public Rigidbody2D rb;

    public float walkSpeed = 5;

    // 1 is right -1 is left
    int _walkDirection = 1;
    public int WalkDirection => _walkDirection;

    [Space, SerializeField]
    UnityEvent OnHitWall;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // FixedUpdate is called once per physics update frame, with a fixed delta time
    void FixedUpdate()
    {
        rb.linearVelocityX = walkSpeed * _walkDirection;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("I collided!", this);

        // If we collided with something relatively wall-shaped, reverse direction
        var contact = collision.GetContact(0);
        float dot = Vector2.Dot(contact.normal, Vector2.right);

        // added a check for contact is not pointing upward (so we dont reverse on corners)
        if (Mathf.Abs(dot) > 0.5f && contact.normal.y < 0.5f)
        {
            OnHitWall.Invoke();
            ReverseDirection();
        }
    }

    public void ReverseDirection()
    {
        _walkDirection = -_walkDirection;
    }
}
