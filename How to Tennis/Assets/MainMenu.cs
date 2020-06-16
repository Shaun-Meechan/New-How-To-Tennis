using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public Player player;
    public SkinLoader skinLoader;
    public TextMeshProUGUI creditsText;
    public GameObject[] menuItems;
    public GameObject[] storeItems;
    private PlayerData playerData;
    void Start()
    {
        //Load the player data and setup the player
        playerData = SaveSystem.LoadPlayer();
        player.credits = playerData.credits;
        player.skin = skinLoader.getSkin(playerData.skinID);

        //Setup the credits text
        creditsText.text = "Credits: " + player.credits;
    }

    public void showStore()
    {
        //Hide menu stuff
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(false);
        }
        //Show store stuff
        for (int i = 0; i < storeItems.Length; i++)
        {
            storeItems[i].SetActive(true);
        }
    }

    public void exitStore()
    {
        //Save data
        SaveSystem.SavePlayer(player);
        //Hide Store
        for (int i = 0; i < storeItems.Length; i++)
        {
            storeItems[i].SetActive(false);
        }
        //Show Menu
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(true);
        }
    }
}
