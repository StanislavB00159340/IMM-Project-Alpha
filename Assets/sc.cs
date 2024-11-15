using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc : MonoBehaviour
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
        transform.Translate(Vector3.right * speed * Time.deltaTime);

        if (transform.position.z < despawnZPosition)
        {

            if (gameObject.CompareTag("Road"))
            {
                RecycleRoadSegment();
            }
            else
            {
                Destroy(gameObject); //alternativly you could recycle the boxes 
            }
        }

    }

    private void RecycleRoadSegment()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, resetZPosition);
    }
}
