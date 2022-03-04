using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopCloseButton : MonoBehaviour
{
    private PlayerManager playerManager;
    void Awake()
    {
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        playerManager.cookieCounter = GameObject.Find("CookieCounter").GetComponent<TMP_Text>();
        playerManager.notEnoughCookiesBanner = GameObject.Find("NotEnoughCookiesBanner");
        DontDestroyOnLoad(GameObject.FindGameObjectWithTag("MainCanvas"));
    }


    public void CloseShop()
    {
        playerManager.gameObject.GetComponent<PlayerMovement>().enabled = true;
    }

    public void BackToMainMenu()
    {
        playerManager.StartLevel("Menü");
    }
}
