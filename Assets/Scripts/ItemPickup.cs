using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    
    
    public enum ItemType
    {
        ExtraBomb,
        ExplosionRadius,
        SpeedIncrease,
    }

    public ItemType type;

    private void OnItemPickup (GameObject player)
    {
        switch (type)
        {
            case ItemType.ExtraBomb:
                player.GetComponent<BombController>().AddBomb();
                break;
            
            case ItemType.ExplosionRadius:
                player.GetComponent<BombController>().explosionRadius++;
                break;

            case ItemType.SpeedIncrease:
                player.GetComponent<MovementController>().movingSpeed += 0.5f;
                break;
        }

        Destroy(gameObject);
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            OnItemPickup(other.gameObject);
        }
    }


}
