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
    public bool burnShot;
    public bool paralyzeShot;
    void Start()
    {
        BoxCollider2D playerCollider = GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), playerCollider);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log(col.gameObject.name + poisonShot + doubleShotArrow);
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
            else if (burnShot)
            {
                col.gameObject.GetComponent<Enemy>().GotDamaged(player.GetComponent<PlayerManager>().burnDmg);
                col.gameObject.GetComponent<Enemy>().Burn(player.GetComponent<PlayerManager>().burnDuration);
            }
            else if (paralyzeShot)
            {
                col.gameObject.GetComponent<Enemy>().GotDamaged(player.GetComponent<PlayerManager>().paralyzeDmg);
                col.gameObject.GetComponent<Enemy>().Paralyze(player.GetComponent<PlayerManager>().paralyzeDuration);
            }
            else
            {
                col.gameObject.GetComponent<Enemy>().GotDamaged(player.GetComponent<PlayerManager>().playerAttackDmg);
            }
        }
        if (!col.gameObject.CompareTag("Cookie") && !col.gameObject.CompareTag("Player") && !col.gameObject.CompareTag("Arrow"))
        {
            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
            Destroy(this.gameObject);
        }
    }
}
