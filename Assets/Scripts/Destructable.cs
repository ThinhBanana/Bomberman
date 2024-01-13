using UnityEngine;

public class Destructible : MonoBehaviour
{


    public float destructionTime = 1f;

    [Range(0f, 1f)]
    public float itemSpawnChance = 0.25f;
    public GameObject[] items;

    private void Start()
    {
        Destroy(gameObject, destructionTime);
    }

    private void OnDestroy()
    {
        if (items.Length > 0 && Random.value < itemSpawnChance)
        {
            int randomItemIndex = Random.Range(0, items.Length);
            Instantiate(items[randomItemIndex], transform.position, Quaternion.identity);
        }
    }


}
