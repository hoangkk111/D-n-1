using UnityEngine;
public class AttackHitbox : MonoBehaviour
{  
    [Range(0, 100)] public int damage = 10;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(tag: "Enemy"))
        {
            Debug.Log("Tấn công Enemy!");
            var health = other.GetComponent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);
        }

        if (other.CompareTag("Player") && gameObject.CompareTag("EnemyAttackHitbox"))
        {
            Debug.Log("Enemy tấn công Player!");
            var health = other.GetComponent<HealthSystem>();
            if (health != null)
                health.TakeDamage(damage);
        }
    }
}
