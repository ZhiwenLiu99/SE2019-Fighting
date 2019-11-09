using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryResctrction : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, -10); 
    }

    // Update is called once per frame
    void Update()
    {
        float leftBound = transform.position.x - 21*transform.localScale.x;
        float rightBound = transform.position.x + 21*transform.localScale.x;
        float bottomBound = transform.position.y - 7*transform.localScale.y;
        float topBound = transform.position.y - 7*transform.localScale.y; 
 
        float camX = Mathf.Clamp(transform.position.x, leftBound, rightBound); 
        float camY = Mathf.Clamp(transform.position.y, bottomBound, topBound);
    }
}
