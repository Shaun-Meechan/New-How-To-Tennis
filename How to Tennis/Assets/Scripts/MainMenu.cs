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
        creditsText.text = "Credits: " + player.credits;
        player.skin = skinLoader.getSkin(playerData.skinID);
        player.firstTime = playerData.firstTime;
        player.skinsOwnedIDs = new int[4];
        //Setup the credits text

        //If it is the players first time playing they will have no skins. No point in loading an array of 0's from disk.
        if (player.firstTime == true)
        {
            player.firstTime = false;
            SaveSystem.SavePlayer(player);
        }
        else
        {
            //Load player skins IDs
            for (int i = 0; i < 4; i++)
            {
                player.skinsOwnedIDs[i] = playerData.skinsOwnedIDs[i];
            }
        }
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
