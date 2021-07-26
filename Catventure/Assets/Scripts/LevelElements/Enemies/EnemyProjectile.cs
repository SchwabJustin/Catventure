using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1;
    public float timeTillSelfDestruct = 2;

    void Start()
    {
        StartCoroutine(SelfDestructTimer());
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(damage);
            Destroy(this.gameObject);
        }
    }
    IEnumerator SelfDestructTimer()
    {
        yield return new WaitForSeconds(timeTillSelfDestruct);
        Destroy(this.gameObject);
    }
}
