using UnityEngine;
using System.IO;
using System.Collections;

public class TranscriptionTranslation : MonoBehaviour
{
    public GameObject boxPrefab; // Assign your box prefab in the inspector
    public float bpm = 100f; // Set this to your BPM
    public float playerDistance = 12f; // Distance from spawn to player
    public TIMEMEASURE timeMeasure; // Reference to TIMEMEASURE script
    public string transcriptionFileName = "spawn_log1.txt"; // The name of the transcription file

    private TranscriptionData[] transcriptArray; // Array to store transcription data
    private int currentDataIndex = 0; // Track the current index in the transcript array

    void Start()
    {
        // Load the transcription data into the array (initialization)
        getTranscript();

        // Start the real-time block spawning process
        StartCoroutine(SpawnBlockCoroutine());
    }

    // Coroutine for spawning blocks in response to dynamic notation changes
    IEnumerator SpawnBlockCoroutine()
    {
        while (true)
        {
            // Continuously check for updates to `Eighthnotes`
            if (currentDataIndex < transcriptArray.Length)
            {
                TranscriptionData currentData = transcriptArray[currentDataIndex];

                // Check if the current timeMeasure's eighth note matches the transcription eighth note
                if (timeMeasure.eighthnotes == currentData.eighthnotes)
                {
                    SpawnBlock(); // Spawn a block at the specified position
                    currentDataIndex++; // Move to the next data point
                }
            }

            // Adjust the wait time as needed for optimal response
            yield return null; // Wait one frame and recheck
        }
    }

    // Method to load transcription data into the array from a file
    void getTranscript()
    {
        string filePath = Path.Combine(Application.persistentDataPath, transcriptionFileName);

        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            transcriptArray = new TranscriptionData[lines.Length];

            // Parse each line and add it to the transcript array
            for (int i = 0; i < lines.Length; i++)
            {
                transcriptArray[i] = ParseTranscriptionLine(lines[i]);
            }
        }
        else
        {
            Debug.LogError($"Transcription file not found at: {filePath}");
        }
    }

    // Parse a line of transcription data
    TranscriptionData ParseTranscriptionLine(string line)
    {
        string[] parts = line.Split(' ');

        // Parse Eighthnotes and other fields
        int eighthNotes = int.Parse(parts[1]);
        int quarterNotes = int.Parse(parts[3]);
        int measure = int.Parse(parts[5]);
        string time = parts[7];

        return new TranscriptionData(eighthNotes, quarterNotes, measure, time);
    }

    // Method to spawn a block at a random position within range
    void SpawnBlock()
    {
        // Randomly choose between the two predefined spawn positions
        Vector3 spawnPosition = (Random.Range(0f, 1f) > 0.5f) ? new Vector3(12, 2, 5) : new Vector3(12, 2, -5);

        // Instantiate the box at the selected position
        GameObject box = Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
        Debug.Log($"Spawned block at eighth note {transcriptArray[currentDataIndex].eighthnotes}");

        // Time offset: The box should be spawned earlier to reach the player on time
        // We calculate the time offset based on the current `eighthnotes` and BPM
        float timeOffset = CalculateTimeOffset(currentDataIndex); // Calculate the correct offset

        // Set movement parameters for the box (adjusted for half-time and offset)
        BoxMovementtwo boxMovement = box.GetComponent<BoxMovementtwo>();
        if (boxMovement != null)
        {
           // boxMovement.SetMovementParameters(bpm, 60f, timeOffset); // 60f for the distance to the player
        }
        else
        {
            Debug.LogError("Box prefab is missing BoxMovementtwo script.");
        }
    }

    // Calculate the time offset based on the current eighth note and BPM
    float CalculateTimeOffset(int index)
    {
        // For simplicity, we assume that each "eighth note" represents a fixed amount of time.
        // You can adjust this formula based on the specific needs of your rhythm.
        float timePerEighthNote = 60f / bpm; // Time per eighth note in seconds
        return index * timePerEighthNote; // Adjust the offset based on index
    }


 


}

// Struct to hold transcription data
public struct TranscriptionData
{
    public int eighthnotes;
    public int quarternotes;
    public int measure;
    public string time;

    public TranscriptionData(int eighthnotes, int quarternotes, int measure, string time)
    {
        this.eighthnotes = eighthnotes;
        this.quarternotes = quarternotes;
        this.measure = measure;
        this.time = time;
    }
}
