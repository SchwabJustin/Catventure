using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Boss2 : MonoBehaviour
{
    private Enemy enemy;
    void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(enemy.damage);
        }
    }
}
