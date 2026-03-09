using UnityEngine;

public class BallDeath : MonoBehaviour
{
    void Start()
    {
        Broadcast.onBallDeath += onDeath;
    }

    void OnDestroy()
    {
        Broadcast.onBallDeath -= onDeath;
    }

    void onDeath(GameObject ball)
    {
        Destroy(gameObject);
    }

}
