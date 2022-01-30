using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss2 : MonoBehaviour
{
    private Enemy enemy;
    public float stunDuration;
    public Color stunColor;
    void Start()
    {
        enemy = transform.parent.GetComponent<Enemy>();

    }

    void FixedUpdate()
    {
        if (enemy.currentHealth <= 0)
        {
            enemy.playerManager.currentCookies += enemy.cookieAmount;
            enemy.playerManager.level2Finished = true;
            SceneManager.LoadScene("Map");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var pm = col.gameObject.GetComponent<PlayerManager>();
            pm.GotDamaged(enemy.damage);
            StartCoroutine(PlayerStun(pm));
        }
    }

    IEnumerator PlayerStun(PlayerManager pm)
    {
        var sr = pm.gameObject.GetComponent<SpriteRenderer>();
        var movement = pm.gameObject.GetComponent<PlayerMovement>();
        movement.enabled = false;
        var oldColor = sr.color;
        sr.color = stunColor;
        yield return new WaitForSeconds(stunDuration);
        movement.enabled = true;
        sr.color = oldColor;
    }
}
