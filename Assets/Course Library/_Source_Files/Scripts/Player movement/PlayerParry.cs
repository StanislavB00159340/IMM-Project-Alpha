using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    // Reference to the Parry GameObject and Animator
    public GameObject parryObject;
    private Animator parryAnimator;

    private bool canParry = false;
    private GameObject currentProjectile;

    private void Start()
    {
        if (parryObject != null)
        {
            parryAnimator = parryObject.GetComponent<Animator>();
        }
    }

    private void Update()
    {
        // Only try to parry if the player is in range of a projectile
        if (canParry && Input.GetKeyDown(KeyCode.Space))
        {
            Parry();
        }
        else if (!canParry && parryAnimator != null)
        {
            // Ensure the parry animation is in idle if no projectile is in range
            parryAnimator.Play("ParryIdle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object collided is tagged as "Projectile"
        if (other.CompareTag("Projectile"))
        {
            canParry = true;
            currentProjectile = other.gameObject;
            // Play the idle animation as a ready stance for parrying
            parryAnimator.Play("ParryIdle");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Reset parry if the projectile leaves the trigger area
        if (other.CompareTag("Projectile"))
        {
            canParry = false;
            currentProjectile = null;
            // Revert to idle animation after the projectile exits
            parryAnimator.Play("ParryIdle");
        }
    }

    private void Parry()
    {
        if (parryAnimator != null)
        {
            // Play "Parry" animation
            parryAnimator.Play("Parry");
        }

        // Destroy the projectile
        if (currentProjectile != null)
        {
            Destroy(currentProjectile);
        }

        // Reset parry status
        canParry = false;
        currentProjectile = null;
    }
}
