using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bark : MonoBehaviour
{
    public int damage;
    public GameObject barkGO;
    public float barkRotationPerSecond;
    public float stunDuration = 1;
    public Color stunColor;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            var pm = col.gameObject.GetComponent<PlayerManager>();
            pm.PlayerStun(stunDuration, stunColor);
            pm.GotDamaged(damage);
        }
    }

    void FixedUpdate()
    {
        if (barkGO.transform.rotation.z >= 90 || barkGO.transform.rotation.z <= -90)
        {
            barkRotationPerSecond = barkRotationPerSecond * -1;
        }
        barkGO.transform.Rotate(Vector3.forward, barkRotationPerSecond * Time.deltaTime);
    }
}
