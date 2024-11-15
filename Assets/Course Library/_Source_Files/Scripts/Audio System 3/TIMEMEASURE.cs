using System;
using UnityEngine;

public class TIMEMEASURE : MonoBehaviour
{
    [Header("Song Settings")]
    [Tooltip("The beats per minute (BPM) of the song.")]
    public float bpm = 100f;

    [Tooltip("The sample rate of the song (in samples per second).")]
    public float sampleRate = 48000f;

    [Header("Display Information")]
    [Tooltip("The current eightnote of the song.")]
    public int eighthnotes = 0;
    [Tooltip("The current quarternote of the song.")]
    public int quarternotes = 0;
    [Tooltip("The current measure of the song.")]
    public int measure = 0;

    [Tooltip("Runtime display string")]
    public string startTimeString;

    private float startTime;
    public float elapsedTime;

    // Time intervals for musical elements
    private float timePerBeat;
    private float timePerQuarterNote;
    private float timePerEighthNote;
    internal int beat;

    public float CurrentTime { get; internal set; }

    void Start()
    {
        startTime = Time.time;

        // Calculate time per beat, quarter note, and eighth note
        timePerBeat = 60f / bpm;
        timePerQuarterNote = timePerBeat;
        timePerEighthNote = timePerBeat / 2f;
    }

    void Update()
    {
        elapsedTime = Time.time - startTime;

        // Update the time string for display
        float minutes = Mathf.Floor(elapsedTime / 60);
        float seconds = Mathf.Floor(elapsedTime % 60);
        float centiseconds = Mathf.Floor((elapsedTime % 1) * 100);
        startTimeString = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, centiseconds);

        // Update the musical elements
        calculateEighthNote();
        calculateQuarterNote();
        calculateMeasure();
    }

    void calculateEighthNote()
    {
        // Increment the step for each 8th note based on the time
        if (elapsedTime >= eighthnotes * timePerEighthNote)
        {
            eighthnotes++;
        }
    }

    void calculateQuarterNote()
    {
        // Increment the beat for each quarter note based on the time
        if (elapsedTime >= quarternotes * timePerQuarterNote)
        {
            quarternotes++;
        }
    }

    void calculateMeasure()
    {
        // A measure typically contains 4 beats (assuming 4/4 time signature)
        if (elapsedTime >= measure * (3 * timePerQuarterNote))
        {
            measure++;
        }
    }
}
