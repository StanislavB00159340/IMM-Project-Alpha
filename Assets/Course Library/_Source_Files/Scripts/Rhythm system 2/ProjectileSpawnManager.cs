using UnityEngine;

public class ProjectileSpawnManager : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign your projectile prefab in the inspector
    public float bpm = 100f; // Set this to your BPM for spawning intervals
    public float amplitudeThreshold = 0.1f; // Set a threshold for projectile spawning
    public AudioSource audioSource; // Assign your AudioSource in the inspector
    public Transform playerTransform; // Assign this in the Inspector to the player object
    public GameObject enemySpeakerLeft; // Assign "ENEMY SPEAKER LEFT" in the inspector
    public GameObject enemySpeakerRight; // Assign "ENEMY SPEAKER RIGHT" in the inspector

    private Animator leftSpeakerAnimator;
    private Animator rightSpeakerAnimator;
    private float spawnInterval;

    private void Start()
    {
        spawnInterval = 60f / bpm;
        leftSpeakerAnimator = enemySpeakerLeft.GetComponent<Animator>();
        rightSpeakerAnimator = enemySpeakerRight.GetComponent<Animator>();

        // Start spawning projectiles on beat
        InvokeRepeating(nameof(SpawnProjectile), 0f, spawnInterval);
    }

    private void SpawnProjectile()
    {
        // Read audio spectrum data and calculate average amplitude
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hamming); // Using Hamming window for accuracy
        float averageAmplitude = CalculateAverageAmplitude(spectrum);

        // Log amplitude for debugging
        Debug.Log($"Current Amplitude: {averageAmplitude:F4}, Threshold: {amplitudeThreshold:F4}");

        // Spawn projectile if amplitude exceeds threshold
        if (averageAmplitude > amplitudeThreshold)
        {
            Vector3 spawnPosition = Random.Range(0, 2) == 0 ? new Vector3(-4f, 10f, 14f) : new Vector3(-4f, 10f, -8f);
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

            // Initialize the projectile to move towards the player
            if (projectile.TryGetComponent(out ProjectileMovement projectileMovement) && playerTransform != null)
            {
                projectileMovement.SetPlayer(playerTransform);
            }

            PlaySpeakerAnimation(spawnPosition);
        }
        else
        {
            SetSpeakersToIdle();
        }
    }

    private float CalculateAverageAmplitude(float[] spectrum)
    {
        float averageAmplitude = 0f;
        foreach (float sample in spectrum)
        {
            averageAmplitude += sample;
        }
        return averageAmplitude / spectrum.Length;
    }

    private void PlaySpeakerAnimation(Vector3 spawnPosition)
    {
        if (spawnPosition.z > 0)
        {
            leftSpeakerAnimator.Play("LeftSpeakerPlay");
            Debug.Log("Left speaker animation triggered.");
        }
        else
        {
            rightSpeakerAnimator.Play("RightSpeakerPlay");
            Debug.Log("Right speaker animation triggered.");
        }
    }

    private void SetSpeakersToIdle()
    {
        leftSpeakerAnimator.Play("Idleleft");
        rightSpeakerAnimator.Play("Idle");
        Debug.Log("Speakers set to idle.");
    }
}
