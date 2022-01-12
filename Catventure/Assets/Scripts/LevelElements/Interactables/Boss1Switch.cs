using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1Switch : MonoBehaviour
{
    public int rockCooldown = 10;
    public GameObject rockPrefab;
    public GameObject cageGO;
    public Sprite cageWithRock;
    public Sprite cageWithoutRock;
    SpriteRenderer spriteRenderer;
    bool rockReady;

    // Start is called before the first frame update

    void Start()
    {
        rockReady = true;
        spriteRenderer = cageGO.GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") && rockReady)
        {
            StartCoroutine(RockAttack());
        }
    }

    IEnumerator RockAttack()
    {
        rockReady = false;
        GameObject currentRock = Instantiate(rockPrefab, cageGO.transform.position, rockPrefab.transform.rotation);
        spriteRenderer.sprite = cageWithoutRock;
        yield return new WaitForSeconds(rockCooldown);
        Destroy(currentRock);
        spriteRenderer.sprite = cageWithRock;
        rockReady = true;
    }
}
