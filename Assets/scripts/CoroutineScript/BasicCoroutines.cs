using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicCoroutines : MonoBehaviour
{
    public int frameNumber = 50;
    void Start()
    {
        StartCoroutine(FrameCount(10));
    }

    IEnumerator FrameCount(int framesToCount)
    {
        Debug.Log("Start frame counter");
        int frames = 0;
        while (frames < framesToCount)
        {
            frames++;
            Debug.Log($"{frames} frames have passed");
            yield return null;
        }
    }

    IEnumerator SecondCount(int secondsToCount)
    {
        Debug.Log("Start frame counter");
        int seconds = 0;
        while (seconds < secondsToCount)
        {
            seconds++;
            Debug.Log($"{seconds} seconds have passed");
            yield return new WaitForSeconds(1);
        }
    }

    IEnumerator ComboRoutine()
    {
        yield return FrameCount(40);
        yield return SecondCount(5);
        Debug.Log("final frame count");
        yield return FrameCount(10);
    }




}
