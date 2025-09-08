using UnityEngine;

public class BuffSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BuffDrop
    {
        public BuffType buffType;
        public GameObject pickupPrefab;
    }

    [Header("Drop Settings")]
    [SerializeField] private BuffDrop[] buffDrops;
    [SerializeField] private float dropChance = 0.15f; // 15%

    public void TrySpawnBuff(Vector3 position)
    {
        if (buffDrops.Length == 0) return;
        
        if (Random.value > dropChance) return;
        
        BuffDrop drop = buffDrops[Random.Range(0, buffDrops.Length)];

        if (drop.pickupPrefab != null)
        {
            GameObject buffGO = Instantiate(drop.pickupPrefab, position, Quaternion.identity);
            
            BuffPickup buffPickup = buffGO.GetComponent<BuffPickup>();
            if (buffPickup != null)
            {
                buffPickup.SetBuffType(drop.buffType);
            }
        }
    }
}