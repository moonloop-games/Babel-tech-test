using System;
using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    public ParticleSystem particle;

    public delegate void BallBounceHandler(GameObject ball, Collision ballCollision);

    public BallBounceHandler onBallBounce;
    void Start()
    {

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log(name + " collided with " + collision.gameObject.name);
        particle.Play();

        Broadcast.onBallBounce?.Invoke(gameObject, collision);
        onBallBounce?.Invoke(gameObject, collision);
    }
}
