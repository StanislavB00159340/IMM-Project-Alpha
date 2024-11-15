using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab; 
    public AudioSource audioSource; 
    public float bpm = 100f;
    public float playerDistance = 12f; 
    public float amplitudeThreshold = 0.1f; 
    public GameObject enemy; 
    private Animator enemyAnimator; 

    private float spawnInterval;

    private void Start()
    {
       
        float beatInterval = 60f / bpm;
        spawnInterval = beatInterval; 

       
        if (enemy != null)
        {
            enemyAnimator = enemy.GetComponent<Animator>();
        }

       
        InvokeRepeating(nameof(SpawnBox), 0f, spawnInterval);
    }

    private void SpawnBox()
    {
        
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        
        float averageAmplitude = 0f;
        for (int i = 0; i < spectrum.Length; i++)
        {
            averageAmplitude += spectrum[i];
        }
        averageAmplitude /= spectrum.Length;

      
        Vector3 spawnPosition = Random.Range(0, 2) == 0 ? new Vector3(-8, 2, 5) : new Vector3(-8, 2, -5);

       
        if (averageAmplitude > amplitudeThreshold)
        {
            GameObject box = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
            BoxMovementtwo boxMovement = box.GetComponent<BoxMovementtwo>();
            if (boxMovement != null)
            {
                boxMovement.SetMovementParameters(bpm, playerDistance);
            }
        }

      
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
