using UnityEngine;

public class MagicSphere : MonoBehaviour
{
    public BouncyBall magicBall;
    void Start()

    {
        magicBall.onBallBounce += DetectMagicBallBounce;
    }

    void OnDestroy()
    {
        magicBall.onBallBounce -= DetectMagicBallBounce;

    }

    void DetectMagicBallBounce(GameObject ball, Collision collision)
    {

        Debug.Log($"Magic ball decector detects that {ball.name} has bounced");
    }

}
