using System.Collections;
using UnityEngine;

public class CoroutineAnimation : MonoBehaviour
{
    // be able to attack a variable a number of times 
    // after attack get hurt then return to idle

    public Animator animator;

    public float idleTime = 2;

    public float attackTime = 2;
    public int numberOfAttacks = 3;

    public float hurtTime = 0.4f;
    void Start()
    {
        StartCoroutine(AnimationRoutine());
    }

    IEnumerator AnimationRoutine()
    {
        yield return new WaitForSeconds(idleTime);
        Debug.Log("idle complete");

        var attackLoop = 0;



        while (numberOfAttacks > attackLoop)
        {
            // attack a number of times
            animator.SetTrigger("Attack");
            attackLoop++;
            yield return new WaitForSeconds(attackTime);
            Debug.Log($"attacked {attackLoop} times!");
        }
        Debug.Log("attack complete");

        var hurtLoop = 2;

        for (var i = 0; i < hurtLoop; i++)
        {
            animator.SetTrigger("Hurt");
            yield return new WaitForSeconds(hurtTime);
            Debug.Log($"played hurt loop {hurtLoop} times!");
        }
        Debug.Log("hurt complete");

        animator.SetTrigger("Idle");

        // scriptable object called ability
        // have variables for 
        // ability duration & cooldown
        // use logs to simulate stuff happening

        // script called character
        // character should be able to execute abilities

    }

}
