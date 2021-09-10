using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRain : MonoBehaviour
{

    public int damage = 30;
    int counter;

    void OnParticleCollision(GameObject col)
    {
        if (col.CompareTag("Enemy"))
        {
            col.gameObject.GetComponent<Enemy>().GotDamaged(damage);
        }
        counter++;

        if (counter >= 4)
        {
            Destroy(gameObject);
        }
    }
}
