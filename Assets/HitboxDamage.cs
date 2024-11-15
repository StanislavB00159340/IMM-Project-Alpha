using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    public HealthController healthController;  // Reference to HealthController on the player
    public float damageAmount = 10f;           // Amount of damage per projectile hit

    private void OnTriggerEnter(Collider other)
    {
        // Only process collisions with objects tagged as "Projectile"
        if (other.CompareTag("Projectile"))
        {
            if (healthController != null)
            {
                healthController.UpdateHealth(damageAmount);
            }

            // Optional: Destroy the projectile after it hits
            Destroy(other.gameObject);
        }
    }
}
