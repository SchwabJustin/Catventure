using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int currentHealth = 1;
    public int maxHealth = 1;
    public int armor = 0;
    private bool _invulnerable;
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
        damage -= armor;
        if (!_invulnerable && damage < 0)
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
}
