using UnityEngine;

public class BoxMovementtwo : MonoBehaviour
{
    private float speed; // Speed of the box
    private float beatInterval; // Time interval between beats
    public AudioClip hitSound; // The sound to play when the box hits the player
    private AudioSource audioSource; // Reference to the AudioSource component

    public void SetMovementParameters(float bpm, float playerDistance)
    {
        // Calculate beat interval based on BPM
        beatInterval = 60f / bpm;

        // Calculate speed needed for the box to reach the player on beat
        speed = playerDistance / beatInterval;

        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component
    }

    private void Update()
    {
        // Move the box towards the player
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerwall"))
        {
            Destroy(gameObject); // Destroy the box when it hits the player wall
        }
        if (other.CompareTag("Player"))
        {
           
           
            Destroy(gameObject); // Destroy the box when it hits the player
        }
    }
}
