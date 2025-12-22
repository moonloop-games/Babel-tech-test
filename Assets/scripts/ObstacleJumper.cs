using System.Collections.Generic;
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
    public float scanDistance = 100f;
    public float raycastDistance;

    [Header("Jump Timing")]
    public float closeJumpDistance;
    public float farJumpDistance;

    private List<ObstacleData> obstaclesRight = new List<ObstacleData>();
    private List<ObstacleData> obstaclesLeft = new List<ObstacleData>();

    private bool wasGrounded = false;

    private struct ObstacleData
    {
        public float distance;
        public float height;
        public Vector2 position;
    }

    void Start()
    {
        CalculateJumpTiming();
        ScanAllObstacles();
    }

    void OnValidate()
    {
        CalculateJumpTiming();
    }

    private void CalculateJumpTiming()
    {
        raycastDistance = jumper.maxJumpDistance * 1.2f;
        closeJumpDistance = jumper.maxJumpDistance * 0.3f;
        farJumpDistance = jumper.maxJumpDistance * 0.6f;
    }

    private void ScanAllObstacles()
    {
        Vector2 character = transform.position;

        // scan right
        obstaclesRight.Clear();
        ScanDirection(character, Vector2.right, obstaclesRight);

        // scan left
        obstaclesLeft.Clear();
        ScanDirection(character, Vector2.left, obstaclesLeft);

        Debug.Log($"Scanned obstacles - Right: {obstaclesRight.Count}, Left: {obstaclesLeft.Count}");
    }

    private void ScanDirection(Vector2 origin, Vector2 direction, List<ObstacleData> obstacleList)
    {
        // get all obstacles in this direction
        RaycastHit2D[] hits = Physics2D.RaycastAll(origin, direction, scanDistance, obstacleLayer);

        // sort them by distance
        System.Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (RaycastHit2D hit in hits)
        {
            if (hit.distance < 0.3f) continue;

            float height = GetObstacleHeight(hit.point);

            ObstacleData data = new ObstacleData
            {
                distance = hit.distance,
                height = height,
                position = hit.point
            };

            obstacleList.Add(data);

            Debug.Log($"Found obstacle at distance {data.distance}, height {data.height} in direction {direction}");

            Debug.DrawLine(origin, hit.point, direction == Vector2.right ? Color.pink : Color.orange, 5f);
        }
    }

    void FixedUpdate()
    {
        if (jumper.isGrounded && !wasGrounded)
        {
            ScanAllObstacles();
        }
        wasGrounded = jumper.isGrounded;

        int direction = patrol.walkDirection;
        Vector2 character = transform.position;
        Vector2 rayDirection = new Vector2(direction, 0);

        List<ObstacleData> currentObstacles = direction > 0 ? obstaclesRight : obstaclesLeft;

        // find the next obstacle we haven't passed yet
        ObstacleData? nextObstacle = null;
        foreach (var obstacle in currentObstacles)
        {
            // calculate current distance to this obstacle based on direction
            float distanceCheck = direction > 0
                ? obstacle.position.x - character.x  // if moving right
                : character.x - obstacle.position.x; // if moving left

            if (distanceCheck > 0.2f) // a small buffer to avoid re-checking the same obstacle
            {
                nextObstacle = obstacle;
                break;
            }
        }

        if (nextObstacle.HasValue)
        {
            ObstacleData obstacle = nextObstacle.Value;
            float currentDistance = direction > 0
                ? obstacle.position.x - character.x
                : character.x - obstacle.position.x;

            Debug.DrawRay(character, rayDirection * currentDistance, Color.red);

            // Check if jumpable
            bool isJumpable = obstacle.height <= jumper.maxJumpHeight;

            Debug.Log($"Next obstacle - Distance: {currentDistance}, Height: {obstacle.height}, Jumpable: {isJumpable}");

            if (isJumpable)
            {
                // Calculate optimal jump distance
                float heightRatio = obstacle.height / jumper.maxJumpHeight;
                float optimalJumpDistance = Mathf.Lerp(farJumpDistance, closeJumpDistance, heightRatio);

                if (currentDistance <= optimalJumpDistance)
                {
                    jumper.Jump();
                }
            }
            else
            {
                // turn around
                if (currentDistance <= 1f)
                {
                    patrol.ReverseDirection();
                }
            }
        }
        else
        {
            Debug.DrawRay(character, rayDirection * scanDistance, Color.green);
        }
    }

    private float GetObstacleHeight(Vector2 hitPoint)
    {
        float maxHeight = 0f;

        // this was specifically for something like triangles where the highest point is different
        // cast multiple rays across a range to find the highest point
        // probably a better way to do this but this works for now
        for (float offset = -1f; offset <= 1f; offset += 0.5f)
        {
            Vector2 rayOrigin = new Vector2(hitPoint.x + offset, transform.position.y + 10f);
            RaycastHit2D topHit = Physics2D.Raycast(rayOrigin, Vector2.down, 20f, obstacleLayer);

            if (topHit)
            {
                float height = topHit.point.y - transform.position.y;
                maxHeight = Mathf.Max(maxHeight, height);

                Debug.DrawRay(rayOrigin, Vector2.down * topHit.distance, Color.yellow);
            }
        }

        return Mathf.Max(0, maxHeight);
    }
}
