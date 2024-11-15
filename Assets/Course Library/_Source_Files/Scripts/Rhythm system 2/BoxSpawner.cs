using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    public GameObject boxPrefab; // Assign your box prefab in the inspector
    public AudioSource audioSource; // Assign your AudioSource in the inspector
    public float bpm = 100f; // Set this to your BPM
    public float playerDistance = 12f; // Distance from spawn to player

    private float spawnInterval;
    private float spawnOffset;

    private void Start()
    {
        // Calculate beat interval and spawn offset based on player distance
        float beatInterval = 60f / bpm;
        spawnInterval = beatInterval; // spawn per beat
        spawnOffset = playerDistance / (1f / beatInterval); // offset for timing

        // Use InvokeRepeating to call SpawnBox with a calculated interval
        InvokeRepeating(nameof(SpawnBox), spawnOffset, spawnInterval);
    }

    private void SpawnBox()
    {
        // Read audio spectrum data to detect beats
        float[] spectrum = new float[256];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        bool kickDetected = spectrum[3] > 0.1f || spectrum[4] > 0.1f || spectrum[5] > 0.1f;
        bool snareDetected = spectrum[8] > 0.1f || spectrum[9] > 0.1f || spectrum[10] > 0.1f;

        // Randomly select a side to spawn the box
        Vector3 spawnPosition = Random.Range(0, 2) == 0 ? new Vector3(12, 2, 5) : new Vector3(12, 2, -5);

        // Spawn a box if either a kick or snare is detected
        if (kickDetected || snareDetected)
        {
            GameObject box = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
            BoxMovementtwo boxMovement = box.GetComponent<BoxMovementtwo>();
            if (boxMovement != null)
            {
                boxMovement.SetMovementParameters(bpm, playerDistance);
            }
        }
    }
}
