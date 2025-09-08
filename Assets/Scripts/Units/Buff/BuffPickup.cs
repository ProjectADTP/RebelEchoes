using UnityEngine;

[RequireComponent(typeof(Collider))]
public class BuffPickup : MonoBehaviour
{
    [SerializeField] private BuffType buffType;
    [SerializeField] private float rotationSpeed = 50f;
    
    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerBuffs playerBuffs = PlayerBuffs.Instance;
            if (playerBuffs != null)
            {
                playerBuffs.AddBuffCharge(buffType);
                Debug.Log($"Подобран бафф: {buffType}");
            }

            Destroy(gameObject);
        }
    }

    public void SetBuffType(BuffType type)
    {
        buffType = type;
    }
}