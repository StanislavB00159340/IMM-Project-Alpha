using UnityEngine;

public class BoxMovementtwo : MonoBehaviour
{
    private float speed;
    private float beatInterval; 
    public AudioClip hitSound; 
    private AudioSource audioSource; 

    public void SetMovementParameters(float bpm, float playerDistance)
    {
       
        beatInterval = 60f / bpm;

       
        speed = playerDistance / beatInterval;

        audioSource = GetComponent<AudioSource>(); 
    }

    private void Update()
    {
       
        transform.Translate(Vector3.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("playerwall"))
        {
            Destroy(gameObject);
        }
        if (other.CompareTag("Player"))
        {


            Destroy(gameObject); 
        }
    }
}