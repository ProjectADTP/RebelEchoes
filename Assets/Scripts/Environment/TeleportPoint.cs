using UnityEngine;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField] private Transform targetPoint;
    
    private void OnTriggerEnter(Collider other)
    {
        if (targetPoint != null && other.gameObject.TryGetComponent(out PlayerHealth player))
        {
            player.gameObject.SetActive(false);
            player.transform.position = targetPoint.position;
            player.gameObject.SetActive(true);
        }
    }
}