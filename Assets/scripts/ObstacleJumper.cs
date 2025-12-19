using Mono.Cecil.Cil;
using UnityEngine;

// Use this component to write your logic for the obstacle jumper.
// Some helpful resources:
// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/Physics2D.Raycast.html
// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/MonoBehaviour.OnCollisionEnter2D.html
// https://docs.unity3d.com/6000.2/Documentation/ScriptReference/LayerMask.html


public class ObstacleJumper : MonoBehaviour
{
    public Rigidbody2D rb;
    public Jumper jumper;
    public Patrol patrol;

    public LayerMask obstacleLayer;

    [Header("Detection Settings")]
    public float raycastDistance;

    [Header("Jump Timing")]
    public float closeJumpDistance;
    public float farJumpDistance;


    void Start()
    {
        CalculateJumpTiming();
    }

    void OnValidate()
    {
        if (jumper != null)
        {
            CalculateJumpTiming();
        }
    }

    private void CalculateJumpTiming()
    {
        raycastDistance = jumper.maxJumpDistance * 1.2f;

        closeJumpDistance = jumper.maxJumpDistance * 0.3f;
        farJumpDistance = jumper.maxJumpDistance * 0.6f;
    }
    void FixedUpdate()
    {
        // get the current direction the patrol is walking
        int direction = patrol.walkDirection;

        // get the current position of the character
        Vector2 character = transform.position;

        // create a direction vector pointing left or right based on direction
        Vector2 rayDirection = new Vector2(direction, 0);

        // cast a ray from character position in the walk direction to detect obstacles
        RaycastHit2D hit = Physics2D.Raycast(character, rayDirection, raycastDistance, obstacleLayer);

        if (hit)
        {
            Debug.DrawRay(character, rayDirection * hit.distance, Color.red);

            // Get the height of the obstacle
            float obstacleHeight = GetObstacleHeight(hit.point);

            // Check if we can jump over it
            bool isJumpable = obstacleHeight > 0 && obstacleHeight <= jumper.maxJumpHeight;

            Debug.Log($"Obstacle Height: {obstacleHeight}, Max Jump Height: {jumper.maxJumpHeight}, Can Jump: {isJumpable}");

            if (isJumpable)
            {
                // Calculate optimal jump distance based on height
                float heightRatio = obstacleHeight / jumper.maxJumpHeight;
                float optimalJumpDistance = Mathf.Lerp(farJumpDistance, closeJumpDistance, heightRatio);

                if (hit.distance <= optimalJumpDistance)
                {
                    jumper.Jump();
                }
            }
            else if (obstacleHeight > jumper.maxJumpHeight)
            {
                if (hit.distance <= 1f)
                {
                    patrol.ReverseDirection();
                }
            }
        }
        else
        {
            Debug.DrawRay(character, rayDirection * raycastDistance, Color.green);
        }

    }

    private float GetObstacleHeight(Vector2 hitPoint)
    {
        // Cast a ray DOWN from above the obstacle to find its top
        Vector2 rayOrigin = new Vector2(hitPoint.x, transform.position.y + 10f);
        RaycastHit2D topHit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f, obstacleLayer);

        if (topHit)
        {
            // The height is the difference between the top of the obstacle and the character's position
            float height = topHit.point.y - transform.position.y;

            // Debug visualization - yellow ray showing the downward cast
            Debug.DrawRay(rayOrigin, Vector2.down * topHit.distance, Color.yellow);

            return Mathf.Max(0, height); // Don't return negative heights
        }

        return 0;
    }
}
