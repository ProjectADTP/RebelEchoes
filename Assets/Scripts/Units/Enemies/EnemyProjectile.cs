using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private float arcHeight;
    private float damage;
    private Vector3 startPosition;
    private float journeyLength;
    private float startTime;
    
    public void Initialize(Transform target, float speed, float arcHeight, float damage)
    {
        this.target = target;
        this.speed = speed;
        this.arcHeight = arcHeight;
        this.damage = damage;
        startPosition = transform.position;
        journeyLength = Vector3.Distance(startPosition, target.position);
        startTime = Time.time;
    }
    
    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
        
        float distCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        
        if (fractionOfJourney >= 1f)
        {
            HitTarget();
            
            return;
        }
        
        Vector3 straightLinePos = Vector3.Lerp(startPosition, target.position, fractionOfJourney);
        float arc = Mathf.Sin(fractionOfJourney * Mathf.PI) * arcHeight;
        Vector3 arcedPos = straightLinePos + Vector3.up * arc;
        
        transform.position = arcedPos;
        
        if (fractionOfJourney > 0.01f)
        {
            Vector3 direction = (arcedPos - transform.position).normalized;
            
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }
        }
    }
    
    private void HitTarget()
    {
        if (target != null && target.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
        }
        
        Destroy(gameObject);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.TakeDamage(damage);
            
            Destroy(gameObject);
        }
    
    }
}