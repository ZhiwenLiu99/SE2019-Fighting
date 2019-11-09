using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterMove : MonoBehaviour
{
    public Transform[] wayPoints;
    public float speed = 0.05f;
    private int index = 0;         //forthcoming point
    public Transform target;

    private void FixedUpdate()
    {
        if (System.Math.Abs(transform.position.x - wayPoints[index].position.x)>0.05)
        {
            Vector2 temp = Vector2.MoveTowards(transform.position, wayPoints[index].position, speed);
            GetComponent<Rigidbody2D>().MovePosition(temp);
        }
        else
        {
            index = (index + 1) % wayPoints.Length;
        }
        Vector2 dir = wayPoints[index].position - transform.position;
        GetComponent<Animator>().SetFloat("DirX", dir.x);
      //  GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == target.name)
        {
            Destroy(collision.gameObject);
        }
    }

}
