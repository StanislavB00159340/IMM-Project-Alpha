using UnityEngine;
using UnityEngine.UI;

public class DashManager : MonoBehaviour
{
    public Slider dashBar; // Reference to the Dash Bar UI slider
    public float dashFillAmount = 0.2f; // Amount to fill dash bar on contact

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collided object has the "pointEnable" tag
        if (other.CompareTag("pointEnable"))
        {
            // Increase the dash bar value, making sure it doesn't exceed the maximum
            dashBar.value = Mathf.Min(dashBar.value + dashFillAmount, dashBar.maxValue);
        }
    }
}
