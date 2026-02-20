using TMPro;
using UnityEditor;
using UnityEngine;

public class Counter : MonoBehaviour
{

    public TMP_Text counterText;
    public int counter = 0;

    void Start()
    {
        counterText.text = counter.ToString();
        Broadcast.onBallBounce += IterateCount;
    }

    void OnDestroy()
    {
        Broadcast.onBallBounce -= IterateCount;
    }

    void IterateCount(GameObject ball, Collision collision)
    {
        Debug.Log(name + " sees that the ball bounced");
        counter++;
        counterText.text = counter.ToString() + " " + ball.name + " was last to bounce!";

        Debug.Log("The velocity of the object is: " + collision.relativeVelocity);

    }


}
