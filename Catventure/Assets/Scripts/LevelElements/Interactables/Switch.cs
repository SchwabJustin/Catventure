using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    public GameObject manipulatedObject;
    public bool objectStatus;

    private bool switchPressed;
    // Start is called before the first frame update
    void Start()
    {
        manipulatedObject.SetActive(objectStatus);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.gameObject.CompareTag("Player")||switchPressed ) return;
        objectStatus = !objectStatus;
        manipulatedObject.SetActive(objectStatus);
        switchPressed = true;
        GetComponent<SpriteRenderer>().color = Color.grey;
    }

}
