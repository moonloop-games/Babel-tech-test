using UnityEngine;

public class ColourChange : MonoBehaviour
{
    void Start()
    {
        Broadcast.onBallChangeColour += ballColourChange;
    }

    void OnDestroy()
    {
        Broadcast.onBallChangeColour -= ballColourChange;
    }

    void ballColourChange(GameObject ball, Material colour)
    {
        Debug.Log($"The ball {ball.name} is now {colour.name}");
    }
}
