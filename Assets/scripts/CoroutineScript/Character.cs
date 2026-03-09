using UnityEngine;

public class Character : MonoBehaviour
{
    public Ability ability;
    void Start()
    {
        StartCoroutine(ability.CharacterRoutine());
    }

    // Update is called once per frame

}
