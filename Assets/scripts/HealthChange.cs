using UnityEngine;

public class HealthChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Broadcast.onBallHealthChange += healthChange;

    }

    // Update is called once per frame
    void OnDestroy()
    {
        Broadcast.onBallHealthChange -= healthChange;
    }

    void healthChange(GameObject ball, int oldHealth, int newHealth)
    {
        if (newHealth == 0)
        {
            Debug.Log($"{ball.name} has died!");

        }
        Debug.Log($"{ball.name} has lost health! It has decreased from {oldHealth} -> {newHealth}");
    }
}
