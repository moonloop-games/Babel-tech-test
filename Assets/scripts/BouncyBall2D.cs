using UnityEngine;

public class BouncyBall2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void OnCollisionEnter2D(Collision2D collision)
    {
        Broadcast.on2DBallBounce?.Invoke(gameObject, collision);
    }
}
