using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject canvasPrefab;
    public Button LoadButton;
    void Start()
    {
        if (!File.Exists(Application.dataPath + "save.txt"))
        {
            LoadButton.interactable = false;
        }
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }

    IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSeconds(4);
        GameObject player = Instantiate(playerPrefab);
        GameObject canvas = Instantiate(canvasPrefab);
        player.GetComponent<PlayerManager>().shouldLoad = false;
    }

    public void LoadGame()
    {
        StartCoroutine(LoadGameCoroutine());


    }
    IEnumerator LoadGameCoroutine()
    {
        yield return new WaitForSeconds(4);
        GameObject player = Instantiate(playerPrefab);
        GameObject canvas = Instantiate(canvasPrefab);
        player.GetComponent<PlayerManager>().shouldLoad = true;
    }
}
