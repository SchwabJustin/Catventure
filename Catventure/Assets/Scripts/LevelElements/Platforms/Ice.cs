using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ice : MonoBehaviour
{
    public float iceDeceleration;
    public float normalDeceleration;
    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerMovement pm = col.gameObject.GetComponent<PlayerMovement>();
            pm.groundDeceleration = iceDeceleration;
        }
    }
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerMovement pm = col.gameObject.GetComponent<PlayerMovement>();
            pm.groundDeceleration = normalDeceleration;
        }
    }
}
