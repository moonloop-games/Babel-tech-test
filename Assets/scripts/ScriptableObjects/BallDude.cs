using UnityEngine;

[CreateAssetMenu(fileName = "new ball dude", menuName = "Scriptable Objects/BallDude")]
public class BallDude : ScriptableObject
{

  public string firstName;
  public string lastName;
  public int age;
  public Material material;

  public string FullName => firstName + " " + lastName;
}
