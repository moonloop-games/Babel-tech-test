using System;
using Unity.VisualScripting;
using UnityEngine;

public class BouncyBall : MonoBehaviour
{
    public ParticleSystem particle;

    //  HOMEWORK :write more delegates & actions 

    // done! colour change, speed & health action 

    // create 2d ball prefab with physics
    // place a few
    // have a manager which is a monobehaviour
    // counts every time a ball bounces
    // every 10 bounces, create a new ball
    public delegate void BallBounceHandler(GameObject ball, Collision ballCollision);

    public BallBounceHandler onBallBounce;

    private float speedThreshold = 5.0f;

    private bool wasAboveThreshold = false;

    public int maxHealth = 3;

    public int currentHealth;




    MeshRenderer meshRenderer;
    Material activeMaterial;

    public Material firstMaterial;
    public Material secondMaterial;

    public BallDude ballDude;

    public Rigidbody rb;


    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        activeMaterial = firstMaterial;
        meshRenderer.material = activeMaterial;
        currentHealth = maxHealth;

        if (ballDude)
        {
            Broadcast.onBallInitialise?.Invoke(ballDude);
            gameObject.name = ballDude.FullName;
            meshRenderer.material = ballDude.material;
        }

    }

    void FixedUpdate()
    {
        float ballSpeed = rb.linearVelocity.magnitude;
        bool isAbove = ballSpeed > speedThreshold;
        if (isAbove && !wasAboveThreshold)
        {
            Broadcast.onBallSpeedChange?.Invoke(gameObject, ballSpeed);
        }
        wasAboveThreshold = isAbove;
    }

    void OnCollisionEnter(Collision collision)
    {
        particle.Play();

        activeMaterial = activeMaterial == firstMaterial ? secondMaterial : firstMaterial;
        meshRenderer.material = activeMaterial;

        Broadcast.onBallChangeColour?.Invoke(gameObject, activeMaterial);
        Broadcast.onBallBounce?.Invoke(gameObject, collision);
        onBallBounce?.Invoke(gameObject, collision);

        if (currentHealth > 0)
        {
            int oldHealth = currentHealth;
            currentHealth--;

            Broadcast.onBallHealthChange?.Invoke(gameObject, oldHealth, currentHealth);
        }
        if (currentHealth == 0)
        {
            Debug.Log("test");
            Broadcast.onBallDeath?.Invoke(gameObject);
        }
    }


}
