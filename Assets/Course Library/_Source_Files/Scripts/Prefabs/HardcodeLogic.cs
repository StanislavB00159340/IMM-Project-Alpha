using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HardcodeLogic : MonoBehaviour
{
    public SpawnManager spawnManager;

   
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            print("Player has entered the trigger zone!");
            spawnManager.SpawnTriggerEntered();
        }
    }
}
