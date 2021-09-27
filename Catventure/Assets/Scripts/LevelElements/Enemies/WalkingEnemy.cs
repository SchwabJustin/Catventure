using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    [Tooltip("First Position the Enemy moves to.")]
    public Transform pos1;
    [Tooltip("Second Position the Enemy moves to.")]
    public Transform pos2;


    Enemy enemy;

    public Transform newPos;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        newPos = pos2;
    }
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, newPos.position, Time.deltaTime * enemy.speed);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(enemy.damage);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log("EnteredTrigger " + col.gameObject.name);
        if (col.transform.parent == transform.parent)
        {
            if (newPos.gameObject.name != pos1.gameObject.name)
            {
                newPos = pos1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                newPos = pos2;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
