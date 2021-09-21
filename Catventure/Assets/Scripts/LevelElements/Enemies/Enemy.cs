﻿using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth = 1;
    public int maxHealth = 1;
    [Tooltip("Damage the Enemy deals")]
    public int damage = 1;
    public int expAmount = 10;
    public int armor = 0;
    public float speed = 1;
    public float slowDown = 0.5F;
    bool slowed;
    bool burned;
    bool paralyzed;
    private bool _invulnerable;
    public float invulnerableTime = 0.2F;
    public GameObject cookiePrefab;
    PlayerManager playerManager;

    public void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }
    public void GotDamaged(int damage)
    {
        Debug.Log("Got Hit with " + damage);
        StartCoroutine(DamageDealt(damage));
        if (currentHealth <= 0)
        {
            playerManager.GetExp(expAmount);
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
        if (!slowed)
        {
            StartCoroutine(SlowDownTimer(duration));
        }
    }
    IEnumerator SlowDownTimer(int duration)
    {
        speed -= slowDown;
        slowed = true;
        yield return new WaitForSeconds(duration);
        slowed = false;
        speed += slowDown;
    }

    public void Burn(int duration)
    {
        if (!burned)
        {
            StartCoroutine(BurnTimer(duration));
        }
    }

    IEnumerator BurnTimer(int duration)
    {
        int oldDmg = damage;
        damage = 0;
        burned = true;
        yield return new WaitForSeconds(duration);
        burned = false;
        damage = oldDmg;
    }

    public void Paralyze(int duration)
    {
        if (!paralyzed)
        {
            StartCoroutine(ParalyzeTimer(duration));
        }
    }
    IEnumerator ParalyzeTimer(int duration)
    {
        int oldDmg = damage;
        float oldSpeed = speed;
        damage = 0;
        speed = 0;
        paralyzed = true;
        yield return new WaitForSeconds(duration);
        paralyzed = false;
        damage = oldDmg;
        speed = oldSpeed;
    }
}
