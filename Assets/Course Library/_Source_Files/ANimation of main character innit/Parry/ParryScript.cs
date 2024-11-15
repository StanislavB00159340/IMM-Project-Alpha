using UnityEngine;

public class ParryScript : MonoBehaviour
{
    public Transform player;  // Reference to the player's Transform

    void Start()
    {
        if (player == null)
        {
            // Find the player object by tag or name if it's not manually assigned
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Set the position of the parry object to the player's position
            transform.position = player.position;
        }
    }
}
