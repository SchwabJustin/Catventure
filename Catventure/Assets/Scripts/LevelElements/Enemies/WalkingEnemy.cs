using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    [Tooltip("First Position the Enemy moves to.")]
    public Transform pos1;
    [Tooltip("Second Position the Enemy moves to.")]
    public Transform pos2;
    [Tooltip("Speed of the Platform")]
    public float speed = 1.0f;
    [Tooltip("Damage the Enemy deals")]
    public int damage = 1;

    Enemy enemy;

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }
    void Update()
    {
        transform.position = Vector3.Lerp(pos1.position, pos2.position, (Mathf.Sin(enemy.speed * Time.time) + 1.0f) / 2.0f);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(damage);
        }
    }
}
