using UnityEngine;

public class HitboxDamage : MonoBehaviour
{
    public HealthController healthController;  
    public float damageAmount = 10f;           

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Projectile"))
        {
            if (healthController != null)
            {
                healthController.UpdateHealth(damageAmount);
            }

           
            Destroy(other.gameObject);
        }
    }
}
