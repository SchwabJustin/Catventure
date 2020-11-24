using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingPlattform : MonoBehaviour
{
    [Tooltip("How long it takes the Platform to disappear")]
    public float timeTillDisappear;

    [Tooltip("How long it takes the Platform to appear again")]
    public float timeTillAppear;

    private bool disappearing;
    private BoxCollider2D boxCol;
    private SpriteRenderer spriteRend;

    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        spriteRend = GetComponent<SpriteRenderer>();
    }
    void OnCollisionEnter2D(Collision2D col)
    {
        if (!disappearing)
        {
            StartCoroutine(Disappear());
        }
    }

    IEnumerator Disappear()
    {
        disappearing = true;
        Debug.Log("Disappear");
        yield return new WaitForSeconds(timeTillDisappear);
        boxCol.enabled = false;
        spriteRend.enabled = false;
        yield return new WaitForSeconds(timeTillAppear);
        boxCol.enabled = true;
        spriteRend.enabled = true;
        disappearing = false;
    }
}
