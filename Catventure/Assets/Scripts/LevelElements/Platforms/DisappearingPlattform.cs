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
        yield return new WaitForSeconds(timeTillDisappear - 0.5f);
        GetComponent<SpriteRenderer>().color -= new Color(0, 0, 0, 0.5F);
        yield return new WaitForSeconds(0.5f);
        boxCol.enabled = false;
        spriteRend.enabled = false;
        yield return new WaitForSeconds(timeTillAppear);
        GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, 0.5F);
        boxCol.enabled = true;
        spriteRend.enabled = true;
        disappearing = false;
    }
}
