using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark : MonoBehaviour
{
    public int damage;
    public GameObject barkGO;
    public float barkRotationPerSecond;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.gameObject.GetComponent<PlayerManager>().GotDamaged(damage);
        }
    }

    void FixedUpdate()
    {
        barkGO.transform.Rotate(Vector3.forward, barkRotationPerSecond * Time.deltaTime);

    }
}
