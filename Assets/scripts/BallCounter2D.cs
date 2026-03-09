using UnityEngine;

public class BallCounter2D : MonoBehaviour
{
    public Vector2 yOffset = new Vector2(0, 10);

    float bounceNumber = 0;


    void Start()
    {
        Broadcast.on2DBallBounce += ballCounter;
    }

    void OnDestroy()
    {
        Broadcast.on2DBallBounce -= ballCounter;

    }

    void ballCounter(GameObject ball, Collision2D collision)
    {
        bounceNumber++;
        if (bounceNumber % 10 == 0)
        {
            Debug.Log($"Balls have bounced by a division of ten");

            Instantiate(ball, Random.insideUnitCircle + yOffset, Quaternion.identity);
        }
        Debug.Log($"Balls have bounced: {bounceNumber} times!");
    }


}
