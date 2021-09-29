using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemy : MonoBehaviour
{
    [Tooltip("First Position the Enemy moves to.")]
    public Transform endPos;
    [Tooltip("Second Position the Enemy moves to.")]
    public float speed = 1.0f;

    [Tooltip("True if player is sighted")]
    public bool playerSighted;
    [Tooltip("Enemy Projectile Prefab")]
    public GameObject enemyProjectilePrefab;
    [Tooltip("Position where Projectile Spawns")]
    public GameObject shootPosition;
    [Tooltip("Time till Enemy can shoot again in Seconds")]
    public float shootForce = -4;
    [Tooltip("Speed of projectile")]
    public float cooldownTime;
    [Tooltip("True if Enemy can shoot")]
    public bool cooldown;
    bool isGoingLeft = false;

    protected Vector3 velocity;
    private Transform _transform;
    public float distance = 1f;
    public float distFromStart;
    Vector3 _originalPosition;
    Enemy enemy;

    public void Start()
    {
        enemy = GetComponent<Enemy>();
        _originalPosition = gameObject.transform.position;
        distance = Vector2.Distance(_originalPosition, endPos.position);
        _transform = GetComponent<Transform>();
        velocity = new Vector3(speed, 0, 0);
        _transform.Translate(velocity.x * Time.deltaTime, 0, 0);
    }
    void Update()
    {
        distFromStart = transform.position.x - _originalPosition.x;

        if (isGoingLeft)
        {
            if (distFromStart >= distance)
                SwitchDirection();

            _transform.Translate(velocity.x * Time.deltaTime * enemy.speed, 0, 0);
        }
        else
        {
            if (distFromStart <= 0)
                SwitchDirection();

            _transform.Translate(-velocity.x * Time.deltaTime * enemy.speed, 0, 0);
        }
    }

    void SwitchDirection()
    {
        shootForce *= -1;
        isGoingLeft = !isGoingLeft;
        _transform.localScale = new Vector3(_transform.localScale.x * -1, 1, 1);
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
            projectile.GetComponent<EnemyProjectile>().damage = enemy.damage;
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