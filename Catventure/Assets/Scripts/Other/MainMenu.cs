using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playerPrefab;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<PlayerManager>().StartLevel("Level1");
    }

    public void LoadGame()
    {
        GameObject player = Instantiate(playerPrefab);
        player.GetComponent<PlayerManager>().LoadGame();
    }
}
