using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerMovement playerMovement;
    public GameObject pauseMenu;

    public Vector3 bossRoom1Pos;
    public Vector3 bossRoom2Pos;
    public Vector3 bossRoom3Pos;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
        pauseMenu = GameObject.Find("PauseMenu");
        GameObject.Find("BackToMainMenu").GetComponent<Button>().onClick.AddListener(delegate { playerManager.StartLevel("Menü"); });

        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
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
            DontDestroyOnLoad(gameObject);
            SceneManager.LoadScene("Map");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if(playerManager.currentScene.Contains("1"))
            {
                playerManager.gameObject.transform.position = bossRoom1Pos;
            }
            if(playerManager.currentScene.Contains("2"))
            {
                playerManager.gameObject.transform.position = bossRoom2Pos;
            }
            if(playerManager.currentScene.Contains("3"))
            {
                playerManager.gameObject.transform.position = bossRoom3Pos;
            }
        }
    }
}
