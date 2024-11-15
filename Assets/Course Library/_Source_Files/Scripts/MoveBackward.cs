using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackward : MonoBehaviour
{
    public float speed = 10f;
    public float resetZPosition = 100f;
    public float despawnZPosition = -10f;


    // Start is called before the first frame update
    void Start()

    {
        Update();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.left * speed * Time.deltaTime);

       
        
    }


}
