using UnityEngine;

public class SnowmanEquip : MonoBehaviour
{
    public Transform hatPos;

    public GameObject hatPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (hatPrefab)
        {
            Instantiate(hatPrefab, hatPos);
        }
    }


}
