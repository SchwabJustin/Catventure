using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool hasHit;
    public GameObject player;
    public bool doubleShotArrow;
    public bool poisonShot;
    void Start()
    {
        BoxCollider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
    }

    // Update is called once per frame
    void Update()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            if (doubleShotArrow)
            {
                col.gameObject.GetComponent<Enemy>().GotDamaged(player.GetComponent<PlayerManager>().doubleShotDmg);

            }
            else if (poisonShot)
            {
                col.gameObject.GetComponent<Enemy>().GotDamaged(player.GetComponent<PlayerManager>().poisonDmg);
                col.gameObject.GetComponent<Enemy>().SlowDown(player.GetComponent<PlayerManager>().poisonDuration);
            }
            else
            {
                col.gameObject.GetComponent<Enemy>().GotDamaged(player.GetComponent<PlayerManager>().playerAttackDmg);
            }
        }
        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
        Destroy(this.gameObject);
    }
}
