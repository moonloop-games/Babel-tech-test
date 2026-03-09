using UnityEngine;
using System.Collections;


[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability")]
public class Ability : ScriptableObject
{
  [SerializeField]
  string abilityName;

  public string AbilityName => abilityName;
  public Sprite abilityIcon;

  public float abilityDuration;
  public float abilityCooldown;

  public IEnumerator CharacterRoutine()
  {
    Debug.Log($"{name} was cast!");
    yield return new WaitForSeconds(abilityDuration);
    Debug.Log($"{name} was completed!");
    yield return new WaitForSeconds(abilityCooldown);

  }

}
