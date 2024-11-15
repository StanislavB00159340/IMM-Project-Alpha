using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodeLogic : MonoBehaviour
{
    public SpawnManager spawnManager;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code can go here if needed
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider has the tag "Player"
        if (other.CompareTag("Player"))
        {
            print("Player has entered the trigger zone!");
            spawnManager.SpawnTriggerEntered();
        }
    }
}
