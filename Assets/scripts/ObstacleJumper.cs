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

    private float raycastDistance => jumper.maxJumpDistance * 1.2f;
    private float closeJumpDistance => jumper.maxJumpDistance * 0.3f;
    private float farJumpDistance => jumper.maxJumpDistance * 0.6f;

    private List<ObstacleData> obstaclesRight = new List<ObstacleData>();
    private List<ObstacleData> obstaclesLeft = new List<ObstacleData>();

    private bool wasGrounded = false;

    private struct ObstacleData
    {
        public float obstacleTop;          // absolute Y position of obstacle top
        public float obstacleStart;        // X where obstacle starts (from our scan direction)
        public float obstacleEnd;          // X where obstacle ends (far edge)
        public Collider2D collider;        // reference to the collider
    }

    void Start()
    {
        ScanAllObstacles();
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

        foreach (RaycastHit2D hit in hits)
        {
            // skip if we've already added this collider
            bool alreadyAdded = false;
            foreach (var obs in obstacleList)
            {
                if (obs.collider == hit.collider)
                {
                    alreadyAdded = true;
                    break;
                }
            }
            if (alreadyAdded) continue;

            Bounds bounds = hit.collider.bounds;

            // determine obstacleStart and obstacleEnd based on scan direction
            float obstacleStart, obstacleEnd;
            if (direction.x > 0) // scanning right
            {
                obstacleStart = bounds.min.x;  // left edge is where it starts
                obstacleEnd = bounds.max.x;    // right edge is where it ends
            }
            else // scanning left
            {
                obstacleStart = bounds.max.x;  // right edge is where it starts 
                obstacleEnd = bounds.min.x;    // left edge is where it ends
            }

            float obstacleTop = bounds.max.y;  // top of the collider

            ObstacleData data = new ObstacleData
            {
                obstacleTop = obstacleTop,
                obstacleStart = obstacleStart,
                obstacleEnd = obstacleEnd,
                collider = hit.collider
            };

            obstacleList.Add(data);

            Debug.Log($"Found obstacle: obstacleStart={obstacleStart}, obstacleEnd={obstacleEnd}, obstacleTop={obstacleTop}");
        }

        // sort by distance from origin
        obstacleList.Sort((a, b) =>
        {
            float distA = Mathf.Abs(a.obstacleStart - origin.x);
            float distB = Mathf.Abs(b.obstacleStart - origin.x);
            return distA.CompareTo(distB);
        });
    }

    void FixedUpdate()
    {
        if (jumper.isGrounded && !wasGrounded)
        {
            ScanAllObstacles();
        }
        wasGrounded = jumper.isGrounded;

        int direction = patrol.WalkDirection;
        Vector2 character = transform.position;
        Vector2 rayDirection = new Vector2(direction, 0);

        List<ObstacleData> currentObstacles = direction > 0 ? obstaclesRight : obstaclesLeft;

        // find current obstacle we're standing on
        ObstacleData? currentPlatform = null;
        if (jumper.isOnObstacle)
        {
            foreach (var obs in obstaclesRight)
            {
                if (character.x >= obs.collider.bounds.min.x && character.x <= obs.collider.bounds.max.x &&
                    Mathf.Abs(character.y - obs.obstacleTop) < 1f)
                {
                    currentPlatform = obs;
                    break;
                }
            }
            if (!currentPlatform.HasValue)
            {
                foreach (var obs in obstaclesLeft)
                {
                    if (character.x >= obs.collider.bounds.min.x && character.x <= obs.collider.bounds.max.x &&
                        Mathf.Abs(character.y - obs.obstacleTop) < 1f)
                    {
                        currentPlatform = obs;
                        break;
                    }
                }
            }
        }

        // find the next obstacle ahead of us
        ObstacleData? nextObstacle = null;
        foreach (var obstacle in currentObstacles)
        {
            float distanceToStart = direction > 0
                ? obstacle.obstacleStart - character.x
                : character.x - obstacle.obstacleStart;

            if (distanceToStart > 0.2f)
            {
                nextObstacle = obstacle;
                break;
            }
        }

        if (nextObstacle.HasValue)
        {
            ObstacleData obstacle = nextObstacle.Value;
            float distanceToStart = direction > 0
                ? obstacle.obstacleStart - character.x
                : character.x - obstacle.obstacleStart;

            Debug.DrawRay(character, rayDirection * distanceToStart, Color.red);

            // calculate height relative to current position
            float heightToJump = obstacle.obstacleTop - character.y;
            bool isJumpable = heightToJump <= jumper.maxJumpHeight;

            Debug.Log($"Next obstacle - Distance: {distanceToStart}, HeightToJump: {heightToJump}, Jumpable: {isJumpable}");

            if (isJumpable)
            {
                float heightRatio = Mathf.Max(0, heightToJump) / jumper.maxJumpHeight;
                float optimalJumpDistance = Mathf.Lerp(farJumpDistance, closeJumpDistance, heightRatio);

                if (distanceToStart <= optimalJumpDistance)
                {
                    jumper.Jump();
                }
            }
            else
            {
                if (distanceToStart <= 1f)
                {
                    patrol.ReverseDirection();
                }
            }
        }
        else if (currentPlatform.HasValue)
        {
            float edgeX = direction > 0 ? currentPlatform.Value.obstacleEnd : currentPlatform.Value.obstacleStart;
            float distanceToEdge = Mathf.Abs(edgeX - character.x);

            Debug.Log($"On platform, distance to edge: {distanceToEdge}");

            if (distanceToEdge <= 0.5f)
            {
                if (IsWallAhead(character, direction))
                {
                    patrol.ReverseDirection();
                }
                else
                {
                    jumper.Jump();
                }
            }
        }
        else
        {
            Debug.DrawRay(character, rayDirection * scanDistance, Color.green);
        }
    }
    private bool IsWallAhead(Vector2 origin, int direction)
    {
        Vector2 rayDirection = new Vector2(direction, 0);
        RaycastHit2D hit = Physics2D.Raycast(origin, rayDirection, 2f);

        return hit && hit.collider.CompareTag("Wall");
    }
}
