using UnityEngine;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
    PlayerManager playerManager;
    PlayerMovement playerMovement;
    public GameObject pauseMenu;

    public Vector3 bossRoom1Pos;
    public Vector3 bossRoom2Pos;
    public Vector3 bossRoom3Pos;

    private Button skillTreeMenuButton;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
        pauseMenu = GameObject.Find("PauseMenu");
        GameObject.Find("BackToMainMenu").GetComponent<Button>().onClick.AddListener(delegate { playerManager.StartLevel("Menü"); });
        skillTreeMenuButton = GameObject.Find("OpenSkillMenu").GetComponent<Button>();

        pauseMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            skillTreeMenuButton.onClick.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseMenu.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            playerManager.GetExp(playerManager.currentLvl * 100);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            playerManager.armor += 99999999;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            playerManager.currentCookies += 100;
            playerManager.cookieCounter.text = playerManager.currentCookies.ToString();
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
            playerManager.StartLevel("Map");
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
