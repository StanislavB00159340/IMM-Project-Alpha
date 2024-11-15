using UnityEngine;
using TMPro;  // Required for TextMeshPro

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI Points;  // Drag your TextMeshPro UI element here
    private int score = 0;
    public int scoreModifer;

    void Start()
    {
        UpdatePoints();  // Initialize the score display
    }
    void Update(){
        UpdatePoints();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object the player collided with has the "PointEnable" tag
        if (other.CompareTag("pointEnable"))
        {
            score +=  scoreModifer;  // Increase the score by 5
            UpdatePoints();  // Update the score display
        }
    }

    // Method to update the score UI text
    void UpdatePoints()
    {
        Points.text = "Score: " + score;
    }
}
