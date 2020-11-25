using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private BoxCollider2D attackCollider;
    public float attackDuration = 0.2F;
    public float meleeAttackCooldown = 1;

    private bool attackReady = true;
    private SpriteRenderer spriteRend;
    
    void Start()
    {
        attackCollider = GetComponent<BoxCollider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
        spriteRend.enabled = false;
        attackCollider.isTrigger = true;
        attackCollider.enabled = false;
    }
    void Update()
    {
        if (attackReady && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(MeleeAttack());
        }

    }

    IEnumerator MeleeAttack()
    {
        attackReady = false;
        spriteRend.enabled = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackDuration);
        attackCollider.enabled = false;
        spriteRend.enabled = false;
        yield return new WaitForSeconds(meleeAttackCooldown);
        attackReady = true;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().GotDamaged(transform.parent.gameObject.GetComponent<PlayerManager>().playerAttackDmg);
        }
    }
}
