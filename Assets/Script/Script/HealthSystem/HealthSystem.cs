using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public bool isDead => currentHealth <= 0;

    public System.Action OnHurt;
    public System.Action OnDeath;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        Debug.Log($"{gameObject.name} bị mất {damage} máu. Còn lại: {currentHealth}");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            OnDeath?.Invoke();
        }
        else
        {
            OnHurt?.Invoke();
        }
    }
}
