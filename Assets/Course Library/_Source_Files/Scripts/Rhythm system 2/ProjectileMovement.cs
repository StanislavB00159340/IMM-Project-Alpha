using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    public float speed = 10f; // Speed of the projectile
    private Transform player; // Reference to the player's transform

    public void SetPlayer(Transform playerTransform)
    {
        player = playerTransform;
    }

    private void Update()
    {
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Move the projectile towards the player
            transform.position += direction * speed * Time.deltaTime;

            // Optionally, make the projectile face the player
            transform.LookAt(player);
        }
        else
        {
            Debug.LogWarning("Player transform is not assigned to the projectile.");
        }
    }
}
