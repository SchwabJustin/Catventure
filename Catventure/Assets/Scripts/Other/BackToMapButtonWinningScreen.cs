
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMapButtonWinningScreen : MonoBehaviour
{
    public float waitTime = 2;
    private GameObject text;

    // Start is called before the first frame update
    void Start()
    {
        text = transform.Find("Text").gameObject;
        text.SetActive(false);
        StartCoroutine(ActivateBackButton());
    }

    IEnumerator ActivateBackButton()
    {
        yield return new WaitForSeconds(waitTime);
        text.SetActive(true);
    }

    public void GetBackToMap()
    {
        SceneManager.LoadScene("Map");
    }

}
