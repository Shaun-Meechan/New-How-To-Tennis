using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    //Variable to store our player
    public Player player;
    //Variable to store our skin loader
    public SkinLoader skinLoader;
    //Variable to store the credits text
    public TextMeshProUGUI creditsText;
    //Array to store all the objects on the main menu
    public GameObject[] menuItems;
    //Array to store all the objects in the store
    public GameObject[] storeItems;
    //Variable to store the player data object.
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
   
        //If it is the players first time playing they will have no skins. No point in loading an array of 0's.
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

    /// <summary>
    /// Function to swap the screen from the main menu to the store
    /// </summary>
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

    /// <summary>
    /// Function to swap the screen from the store to the main menu
    /// </summary>
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
