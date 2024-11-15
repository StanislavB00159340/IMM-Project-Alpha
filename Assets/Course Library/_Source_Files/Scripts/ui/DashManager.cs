using UnityEngine;
using UnityEngine.UI;

public class DashManager : MonoBehaviour
{
    public Slider dashBar; 
    public float dashFillAmount = 0.2f; 

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("pointEnable"))
        {
           
            dashBar.value = Mathf.Min(dashBar.value + dashFillAmount, dashBar.maxValue);
        }
    }
}
