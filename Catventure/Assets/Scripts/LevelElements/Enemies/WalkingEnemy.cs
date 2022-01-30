using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingEnemy : MonoBehaviour
{
    [Tooltip("First Position the Enemy moves to.")]
    public Transform pos1;
    [Tooltip("Second Position the Enemy moves to.")]
    public Transform pos2;

    private Animator anim;
    Enemy enemy;

    public Transform newPos;

    void Start()
    {
        enemy = GetComponent<Enemy>();
        newPos = pos2;
        anim = GetComponentInChildren<Animator>();
    }
    void FixedUpdate()
    {
        if (!anim.GetBool("Dead") && !anim.GetBool("Hit"))
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos.position, Time.deltaTime * enemy.speed);
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            StartCoroutine(HitAnimation());
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(enemy.damage);
        }
    }

    IEnumerator HitAnimation()
    {
        anim.SetBool("Hit", true);
        yield return new WaitForSeconds(1);
        anim.SetBool("Hit", false);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.transform.parent == transform.parent)
        {
            if (newPos.gameObject.name != pos1.gameObject.name)
            {
                newPos = pos1;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else
            {
                newPos = pos2;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
