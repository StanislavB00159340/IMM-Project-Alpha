using UnityEngine;

public class PlayerParry : MonoBehaviour
{
    
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
       
        if (canParry && Input.GetKeyDown(KeyCode.Space))
        {
            Parry();
        }
        else if (!canParry && parryAnimator != null)
        {
            
            parryAnimator.Play("ParryIdle");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Projectile"))
        {
            canParry = true;
            currentProjectile = other.gameObject;
           
            parryAnimator.Play("ParryIdle");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Projectile"))
        {
            canParry = false;
            currentProjectile = null;
           
            parryAnimator.Play("ParryIdle");
        }
    }

    private void Parry()
    {
        if (parryAnimator != null)
        {
            
            parryAnimator.Play("Parry");
        }

       
        if (currentProjectile != null)
        {
            Destroy(currentProjectile);
        }

       
        canParry = false;
        currentProjectile = null;
    }
}
