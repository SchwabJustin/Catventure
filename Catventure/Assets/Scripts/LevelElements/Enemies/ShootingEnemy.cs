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
    public float shootForce = 4;
    [Tooltip("Speed of projectile")]
    public float cooldownTime;
    [Tooltip("True if Enemy can shoot")]
    public bool cooldown;
    void Update()
    {
        if (!playerSighted)
        {
            transform.position = Vector3.Lerp(pos1.position, pos2.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
        }
        if (transform.position == pos1.position)
        {
            transform.localScale = Vector3.one;
        }
        if (transform.position == pos2.position)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            Shoot(col.gameObject.transform);
        }
    }

    public void Shoot(Transform playerTransform)
    {
        if (!cooldown)
        {
            cooldown = true;
            var projectile = Instantiate(enemyProjectilePrefab, shootPosition.transform);
            projectile.GetComponent<Rigidbody2D>().AddForce(Vector2.MoveTowards(shootPosition.transform.position, playerTransform.position, 3) * shootForce);
            StartCoroutine(ResetCooldown());
        }
    }
    IEnumerator ResetCooldown()
    {
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }
}