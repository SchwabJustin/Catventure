using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShowTextCol : MonoBehaviour
{
    public GameObject[] children;
    void Start()
    {
        children = new GameObject[transform.childCount];
        for (int i = 0; transform.childCount > i; i++)
        {
            children[i] = transform.GetChild(i).gameObject;
            children[i].SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            foreach (GameObject child in children)
            {
                child.SetActive(true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            foreach (GameObject child in children)
            {
                child.SetActive(false);
            }
        }
    }
}
