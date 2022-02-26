using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            enemy.playerManager.StartLevel("Map");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var pm = col.gameObject.GetComponent<PlayerManager>();
            pm.PlayerStun(stunDuration, stunColor);
            pm.GotDamaged(enemy.damage);
        }
    }

}
