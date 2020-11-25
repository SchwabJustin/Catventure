using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [Tooltip("First Position the Platform moves to.")]
    public Transform pos1;
    [Tooltip("Second Position the Platform moves to.")]
    public Transform pos2;
    [Tooltip("Speed of the Platform")]
    public float speed = 1.0f;

    void Update()
    {
        transform.position = Vector3.Lerp(pos1.position, pos2.position, (Mathf.Sin(speed * Time.time) + 1.0f) / 2.0f);
    }
    private void OnTriggerStay2D(Collider2D collider)
    {
        collider.gameObject.transform.SetParent(transform);
    }

    private void OnTriggerExit2D(Collider2D collider)
    {
        collider.gameObject.transform.SetParent(null);
    }
}