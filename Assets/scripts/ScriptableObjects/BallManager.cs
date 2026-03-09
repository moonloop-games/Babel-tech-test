using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{

  void onBallInitialise(BallDude ball)
  {
    Debug.Log($"Manager sees the {ball.name} is initialised");
  }

  void Start()
  {
    Broadcast.onBallInitialise += onBallInitialise;
  }

  void OnDestroy()
  {
    Broadcast.onBallInitialise -= onBallInitialise;
  }

}
