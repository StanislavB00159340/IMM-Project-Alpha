using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab; // Assign your box prefab in the inspector
    public AudioSource audioSource; // Assign your AudioSource in the inspector
    public float bpm = 100f; // Set this to your BPM
    public float playerDistance = 12f; // Distance from spawn to player
    public float amplitudeThreshold = 0.1f; // Set a threshold for box spawning
    public GameObject enemy; // Assign your enemy GameObject in the inspector
    private Animator enemyAnimator; // Enemy Animator component

    private float spawnInterval;

    private void Start()
    {
        // Calculate beat interval
        float beatInterval = 60f / bpm;
        spawnInterval = beatInterval; // spawn per beat

        // Get the Animator component from the enemy GameObject
        if (enemy != null)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
        }

        // Start spawning boxes without an initial offset
        InvokeRepeating(nameof(SpawnBox), 0f, spawnInterval); // Set initial delay to 0
    }

    private void SpawnBox()
    {
        // Read audio spectrum data
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        // Calculate the average amplitude across all spectrum data points
        float averageAmplitude = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            averageAmplitude += spectrum[i];
        }
        averageAmplitude /= spectrum.Length;

        // Randomly select a side to spawn the box and decide animation
        Vector3 spawnPosition = Random.Range(0, 2) == 0 ? new Vector3(-8, 2, 5) : new Vector3(-8, 2, -5);

        // Check if the average amplitude exceeds the threshold to spawn a box
        if (averageAmplitude > amplitudeThreshold)
        {
            GameObject box = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
            BoxMovementtwo boxMovement = box.GetComponent<BoxMovementtwo>();
            if (boxMovement != null)
            {
                boxMovement.SetMovementParameters(bpm, playerDistance);
            }
        }

        // Play the appropriate animation on the enemy based on spawn position
        if (enemyAnimator != null)
        {
            if (spawnPosition.z > 0)
            {
                enemyAnimator.Play("BoomboxLeft");
            }
            else
            {
                enemyAnimator.Play("BoomboxRight");
            }
        }
    }
}
