using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [Tooltip("First Position the Enemy moves to.")]
    public Transform pos1;
    [Tooltip("Second Position the Enemy moves to.")]
    public Transform pos2;
    [Tooltip("Speed of the Platform")]
    public float speed = 1.0f;
    [Tooltip("Damage the Enemy deals")]
    public int damage = 1;
    [Tooltip("True if player is sighted")]
    public bool playerSighted;
    [Tooltip("Enemy Projectile Prefab")]
    public GameObject enemyProjectilePrefab;
    [Tooltip("Position where Projectile Spawns")]
    public GameObject shootPosition;
    [Tooltip("Time till Enemy can shoot again in Seconds")]
    public float cooldownTime;
    [Tooltip("True if Enemy can shoot")]
    private bool cooldown;
    void Update()
    {
        if (!playerSighted)
        {
            transform.position = Vector3.Lerp(pos1.position, pos2.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Shoot();
        }
    }

    public void Shoot()
    {
        if (cooldown)
        {
            cooldown = false;
            Instantiate(enemyProjectilePrefab, shootPosition.transform);
        }
    }
}