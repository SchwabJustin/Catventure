using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockBoss1 : MonoBehaviour
{
    public int rockDmg;
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            col.GetComponent<Enemy>().GotDamaged(rockDmg);
        }
    }
}
