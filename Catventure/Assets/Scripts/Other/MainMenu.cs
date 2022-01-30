using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject canvasPrefab;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        GameObject player = Instantiate(playerPrefab);
        GameObject canvas = Instantiate(canvasPrefab);
        player.GetComponent<PlayerManager>().shouldLoad = false;
    }

    public void LoadGame()
    {
        GameObject player = Instantiate(playerPrefab);
        GameObject canvas = Instantiate(canvasPrefab);
        player.GetComponent<PlayerManager>().shouldLoad = true;
    }
}
