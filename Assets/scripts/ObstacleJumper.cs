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

    // in a fixedUpdate, check for obstacles in front of the patrol using a raycast

    // there is always going to be one, so we calculate the distance to it

    // then we detect height and see if its jumpable

    // if it is jumpable with the distance provided, we make the jumper jump

    // else we turn the patrol around


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
