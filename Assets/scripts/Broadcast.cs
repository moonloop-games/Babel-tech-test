using System;
using UnityEngine;

public static class Broadcast
{
    public static BouncyBall.BallBounceHandler onBallBounce;

    public static Action<BallDude> onBallInitialise;

    public static Action<Ability> onCharacterInitialise;

    public static Action<GameObject, Material> onBallChangeColour;

    public static Action<GameObject, float> onBallSpeedChange;

    public static Action<GameObject, int, int> onBallHealthChange;

    public static Action<GameObject> onBallDeath;

    public static Action<GameObject, Collision2D> on2DBallBounce;

}
