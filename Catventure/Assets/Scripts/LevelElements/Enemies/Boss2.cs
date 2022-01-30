using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Boss2 : MonoBehaviour
{
    private Enemy enemy;
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
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(enemy.damage);
        }
    }
}
