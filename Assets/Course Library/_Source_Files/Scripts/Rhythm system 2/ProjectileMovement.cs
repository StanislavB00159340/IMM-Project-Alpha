using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10f; 
    private Transform player; 

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Update()
    {
        if (player != null)
        {
            
            Vector3 direction = (player.position - transform.position).normalized;

           
            transform.position += direction * speed * Time.deltaTime;

           
            transform.LookAt(player);
        }
        else
        {
            Debug.LogWarning("Player transform is not assigned to the projectile.");
        }
    }
}
