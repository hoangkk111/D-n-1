using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    public float damage = 20f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // Gọi phương thức TakeDamage của PlayerController
           // PlayerController player = collision.GetComponent<PlayerController>();
           // if (player != null)
            {
              //  player.TakeDamage(damage);
            }
        }
    }
}
