using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlat : MonoBehaviour
{
    public List<Transform> WayPoints;
    public int index;
    public float speed;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, WayPoints[index].position, speed * Time.deltaTime);
    }

    private void FixedUpdate()
    {
        if (transform.position == WayPoints[index].position) {
            if (index == WayPoints.Count - 1)
            {
                index = 0;
            }
            else { 
                index++;
            }
        }
    }
}
