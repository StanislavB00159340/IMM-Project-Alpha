using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackward : MonoBehaviour
{
    public float speed = 10f;
    public float resetZPosition = 100f;
    public float despawnZPosition = -10f;


    
    void Start()

    {
        Update();
    }

   
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

       
        
    }


}
