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
    public float raycastDistance = 5f;
    public float maxJumpableHeight = 2f;

    [Header("Jump Timing")]
    public float closeJumpDistance;
    public float farJumpDistance;
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

        // when we hit something we check it
        if (hit)
        {
            // get how far away the obstacle is from the character
            float distanceToObstacle = hit.distance;

            // then we detect height and see if its jumpable
            // calculate the height of the obstacle using an upward raycast
            float obstacleHeight = GetObstacleHeight(hit.point);

            // check if the obstacle height is within our jumping capability
            bool isJumpable = obstacleHeight <= maxJumpableHeight;

            // if it is jumpable with the distance provided, we make the jumper jump
            if (isJumpable)
            {
                // TODO figure out this jump timing logic
                float jumpDistance = 2;

                if (distanceToObstacle <= jumpDistance)
                {
                    jumper.Jump();
                }
            }
        }
    }

    // TODO implement a function to determine to tell patrol to turn around if obstacle is not jumpable


    private float GetObstacleHeight(Vector2 hitPoint)
    {
        Vector2 origin = new Vector2(hitPoint.x, transform.position.y);

        // cast a ray straight up from the base to find the top of the obstacle
        RaycastHit2D upwardHit = Physics2D.Raycast(origin, Vector2.up, 10f, obstacleLayer);

        if (upwardHit)
        {
            // calculate height as difference between obstacle top and character position
            float height = upwardHit.point.y - transform.position.y;

            return Mathf.Max(0, height);
        }
        return 0;
    }
}
