using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private List<GameObject> switchGOs = new List<GameObject>();
    // Start is called before the first frame update

    void Start()
    {
        rockReady = true;
        spriteRenderer = cageGO.GetComponent<SpriteRenderer>();
        switchGOs = GameObject.FindGameObjectsWithTag("BossSwitch").ToList();
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
        foreach (GameObject go in switchGOs)
        {
            go.GetComponent<Boss1Switch>().rockReady = false;
            go.GetComponent<Boss1Switch>().GetComponent<SpriteRenderer>().color = Color.grey;
            go.GetComponent<Boss1Switch>().spriteRenderer.sprite = cageWithoutRock;

        }
        GameObject currentRock = Instantiate(rockPrefab, cageGO.transform.position, rockPrefab.transform.rotation);
        yield return new WaitForSeconds(rockCooldown);
        Destroy(currentRock);
        foreach (GameObject go in switchGOs)
        {
            go.GetComponent<Boss1Switch>().spriteRenderer.sprite = cageWithRock;
            go.GetComponent<Boss1Switch>().rockReady = true;
            go.GetComponent<Boss1Switch>().GetComponent<SpriteRenderer>().color = Color.white;
        }
    }
}
