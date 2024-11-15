using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NewSpawnScript : MonoBehaviour
{
    // Set your prefab for spawning
    public GameObject boxPrefab;

    // List to hold parsed spawn events
    private List<SpawnEvent> spawnEvents = new List<SpawnEvent>();

    // Reference to the TIMEMEASURE object
    public TIMEMEASURE timeMeasure;

    void Start()
    {
        // Load data from file and parse
        string filePath = Path.Combine(Application.persistentDataPath, "spawn_log1.txt");
        ParseNotationFile(filePath);

        // Start the spawn monitoring coroutine
        StartCoroutine(SpawnQueueMonitor());
    }

    // Method to parse notation file
    void ParseNotationFile(string filePath)
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                SpawnEvent spawnEvent = ParseLineToSpawnEvent(line);
                if (spawnEvent != null)
                {
                    spawnEvents.Add(spawnEvent);
                }
            }
        }
        else
        {
            Debug.LogError("File not found: " + filePath);
        }
    }

    // Parse each line into a SpawnEvent
    SpawnEvent ParseLineToSpawnEvent(string line)
    {
        try
        {
            // Expected line format: "Eighthnotes: 5 Quarternotes: 3 Measure: 1 Current Time: 00:01:20"
            string[] parts = line.Split(' ');

            if (parts.Length >= 8)
            {
                // Parse integer components
                if (int.TryParse(parts[1], out int eighthnotes) &&
                    int.TryParse(parts[3], out int quarternotes) &&
                    int.TryParse(parts[5], out int measure))
                {
                    // Parse time components
                    string[] timeParts = parts[7].Split(':');

                    // Check if we have exactly 3 parts (minutes, seconds, centiseconds)
                    if (timeParts.Length == 3)
                    {
                        // Try parsing each part and handle culture-invariant parsing
                        if (float.TryParse(timeParts[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float minutes) &&
                            float.TryParse(timeParts[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float seconds) &&
                            float.TryParse(timeParts[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float centiseconds))
                        {
                            // Check valid range
                            if (seconds < 60 && centiseconds < 100)
                            {
                                // Convert to total seconds
                                float spawnTime = (minutes * 60) + seconds + (centiseconds / 100);
                                return new SpawnEvent(eighthnotes, quarternotes, measure, spawnTime);
                            }
                            else
                            {
                                Debug.LogError($"Time components out of range in line: {line}");
                            }
                        }
                        else
                        {
                            Debug.LogError($"Failed to parse time components as floats in line: {line}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"Time format is incorrect in line: {line}");
                    }
                }
                else
                {
                    Debug.LogError($"Failed to parse eighthnotes, quarternotes, or measure in line: {line}");
                }
            }
            else
            {
                Debug.LogError($"Line format is incorrect: {line}");
            }
        }
        catch (FormatException e)
        {
            Debug.LogError($"FormatException in ParseLineToSpawnEvent: {e.Message} | Line: {line}");
        }
        catch (Exception e)
        {
            Debug.LogError($"Unexpected error in ParseLineToSpawnEvent: {e.Message} | Line: {line}");
        }
        return null;
    }



    // Coroutine to monitor and spawn boxes at the right time
    IEnumerator SpawnQueueMonitor()
    {
        while (true)
        {
            for (int i = spawnEvents.Count - 1; i >= 0; i--)
            {
                SpawnEvent spawnEvent = spawnEvents[i];

                // Check if current elapsed time and TIMEMEASURE values match the spawn event
                if (Mathf.Approximately(timeMeasure.elapsedTime, spawnEvent.spawnTime)
                    && timeMeasure.eighthnotes >= spawnEvent.eighthnotes
                    && timeMeasure.quarternotes >= spawnEvent.quarternotes
                    && timeMeasure.measure >= spawnEvent.measure)
                {
                    SpawnBox();
                    spawnEvents.RemoveAt(i); // Remove after spawning
                }
            }
            yield return null; // Check each frame
        }
    }

    // Method to spawn box at designated positions
    void SpawnBox()
    {
        Vector3 spawnPosition = UnityEngine.Random.Range(0, 2) == 0 ? new Vector3(12, 2, 5) : new Vector3(12, 2, -5);
        Instantiate(boxPrefab, spawnPosition, Quaternion.identity);
    }
}

// Event data structure to hold parsed data
[System.Serializable]
public class SpawnEvent
{
    public int eighthnotes;
    public int quarternotes;
    public int measure;
    public float spawnTime; // in seconds

    public SpawnEvent(int eighthnotes, int quarternotes, int measure, float spawnTime)
    {
        this.eighthnotes = eighthnotes;
        this.quarternotes = quarternotes;
        this.measure = measure;
        this.spawnTime = spawnTime;
    }
}
