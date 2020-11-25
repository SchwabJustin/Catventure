using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth = 1;

    public int maxHealth = 1;
    private bool invulnerable;
    public float invulnerableTime = 0.2F;

    public void GotDamaged(int damage)
    {
        Debug.Log("Got Hit");
        StartCoroutine(DamageDealt(damage));
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    public IEnumerator DamageDealt(int damage)
    {
        if (!invulnerable)
        {
            currentHealth -= damage;
            invulnerable = true;
        }
        yield return new WaitForSeconds(invulnerableTime);
        if (invulnerable)
        {
            invulnerable = false;
        }
    }
}
