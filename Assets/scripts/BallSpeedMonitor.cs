using UnityEngine;

public class BallSpeedMonitor : MonoBehaviour
{
    void Start()
    {
        Broadcast.onBallSpeedChange += ballSpeedMonitor;
    }

    void OnDestroy()
    {
        Broadcast.onBallSpeedChange -= ballSpeedMonitor;

    }

    void ballSpeedMonitor(GameObject ball, float speed)
    {
        Debug.Log($"{ball.name} is travelling too fast at {speed}");
    }
}
