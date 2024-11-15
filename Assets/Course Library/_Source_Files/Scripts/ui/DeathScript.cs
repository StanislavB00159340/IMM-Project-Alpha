using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class Deathscript : MonoBehaviour
{   
     public TextMeshProUGUI textComponent;    
    public Slider healthBar;
    public GameObject player;

    void Update()
    {
       
        if (healthBar.value <= 0 && player != null)
        {
          
            Destroy(player);
             textComponent.text = "YOU DIED";
            
        }
    }
}
