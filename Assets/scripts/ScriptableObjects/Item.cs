using Unity.Mathematics;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Scriptable Objects/Item")]
public class Item : ScriptableObject
{

  [SerializeField]
  string itemName;

  public string ItemName => itemName;

  public int durability;

  public int price;

  public float timeAccquired;

  public Sprite itemIcon;

  void OnValidate()
  {
    durability = Mathf.Clamp(durability, 0, 500);
    price = Mathf.Max(price, 0);
  }
}
