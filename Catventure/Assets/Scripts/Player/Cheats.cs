using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cheats : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerMovement playerMovement;
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            playerManager.currentSkillPoints++;
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            playerManager.currentPlayerHealth += 99999999;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            playerManager.currentCookies += 100;
            playerManager.cookieCounter.text = "Cookies: " + playerManager.currentCookies;
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            playerMovement.jumpHeight *= 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            playerMovement.jumpHeight /= 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            playerManager.level1Finished = true;
            playerManager.level2Finished = true;
            SceneManager.LoadScene("Map");
        }
    }
}
