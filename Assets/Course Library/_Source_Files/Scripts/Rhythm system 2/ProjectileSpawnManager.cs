using UnityEngine;

public class ProjectileSpawnManager : MonoBehaviour
{
    public GameObject projectilePrefab; 
    public float bpm = 100f; 
    public float amplitudeThreshold = 0.1f; 
    public AudioSource audioSource; 
    public Transform playerTransform; 
    public GameObject enemySpeakerLeft; 
    public GameObject enemySpeakerRight; 

    private Animator leftSpeakerAnimator;
    private Animator rightSpeakerAnimator;
    private float spawnInterval;

    private void Start()
    {
        spawnInterval = 60f / bpm;
        leftSpeakerAnimator = enemySpeakerLeft.GetComponent<Animator>();
        rightSpeakerAnimator = enemySpeakerRight.GetComponent<Animator>();

        
        InvokeRepeating(nameof(SpawnProjectile), 0f, spawnInterval);
    }

    private void SpawnProjectile()
    {
       
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hamming);
        float averageAmplitude = CalculateAverageAmplitude(spectrum);

       
        Debug.Log($"Current Amplitude: {averageAmplitude:F4}, Threshold: {amplitudeThreshold:F4}");

        
        if (averageAmplitude > amplitudeThreshold)
        {
            Vector3 spawnPosition = Random.Range(0, 2) == 0 ? new Vector3(-4f, 10f, 14f) : new Vector3(-4f, 10f, -8f);
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, Quaternion.identity);

           
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
