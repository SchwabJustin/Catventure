using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    [Tooltip("First Position the Enemy moves to.")]
    public Transform pos1;
    [Tooltip("Second Position the Enemy moves to.")]
    public Transform pos2;

    [Tooltip("Damage the Enemy deals")]
    public int damage = 1;

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
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(damage);
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
            }
            else
            {
                newPos = pos2;
            }
        }
    }
}
