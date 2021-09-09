using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth = 1;
    public int maxHealth = 1;
    public int armor = 0;
    public float speed = 1;
    public float slowDown = 0.5F;
    private bool _invulnerable;
    public float invulnerableTime = 0.2F;
    public GameObject cookiePrefab;
    public void GotDamaged(int damage)
    {
        Debug.Log("Got Hit with " + damage);
        StartCoroutine(DamageDealt(damage));
        if (currentHealth <= 0)
        {
            Vector3 currentPosition = transform.position;
            GameObject cookie = Instantiate(cookiePrefab);
            cookie.transform.position = transform.position;
            Destroy(this.gameObject);
        }
    }
    public IEnumerator DamageDealt(int damage)
    {
        damage -= armor;
        if (!_invulnerable && damage > 0)
        {
            currentHealth -= damage;
            _invulnerable = true;
        }
        yield return new WaitForSeconds(invulnerableTime);
        if (_invulnerable)
        {
            _invulnerable = false;
        }
    }

    public void SlowDown(int duration)
    {
        StartCoroutine(SlowDownTimer(duration));
    }

    IEnumerator SlowDownTimer(int duration)
    {
        speed -= slowDown;
        yield return new WaitForSeconds(duration);
        speed += slowDown;
    }
}
