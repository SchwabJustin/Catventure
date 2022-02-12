using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MapManager : MonoBehaviour
{
    PlayerManager playerManager;
    public GameObject desertButton;
    public GameObject iceButton;

    void Start()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        if (playerManager.level1Finished)
        {
            desertButton.SetActive(true);
        }
        else
        {
            desertButton.SetActive(false);
        }
        if (playerManager.level2Finished)
        {
            iceButton.SetActive(true);
        }
        else
        {
            iceButton.SetActive(false);
        }
    }

    // Update is called once per frame
    public void LoadLevel(string levelName)
    {
        playerManager.StartLevel(levelName);
    }
}
