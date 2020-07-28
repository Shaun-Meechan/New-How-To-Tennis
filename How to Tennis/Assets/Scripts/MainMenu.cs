using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    //Variable to store our version text object
    public TextMeshProUGUI versionText;
    //Variables for the audio button
    public Image audioButton;
    public Sprite audioPlayingSprite;
    public Sprite audioMutedSprite;
    public AudioToggle audioToggle;
    void Start()
    {
        Application.targetFrameRate = 60;
        //Check to see if a save file exists
        if (SaveSystem.DoesFileExist() == false)
        {
            SaveSystem.SavePlayer(player);
            Start();
        }
        else
        {
            //Setup the version text
            versionText.text = "Version: " + Application.version;
            //Load the player data and setup the player
            playerData = SaveSystem.LoadPlayer();
            player.credits = playerData.credits;
            creditsText.text = "Credits: " + player.credits;
            player.skinID = playerData.skinID;
            player.skin = skinLoader.getSkin(playerData.skinID);
            player.firstTime = playerData.firstTime;
            player.skinsOwnedIDs = new int[6];
            player.playAudio = playerData.playAudio;

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

            //Set the audio button to the correct value
            if (player.getPlayAudio() == true)
            {
                //Show playing audio icon
                audioButton.sprite = audioPlayingSprite;
                audioToggle.playAudio();
            }
            else
            {
                //Show muted audio icon
                audioButton.sprite = audioMutedSprite;
                audioToggle.muteAudio();
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
    /// Sets all objcts in the array 'menuItems'.setActive to false 
    /// </summary>
    public void hideMainMenu()
    {
        //Hide menu stuff
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(false);
        }
    }

    /// <summary>
    /// Sets all objcts in the array 'menuItems'.setActive to true 
    /// </summary>
    public void showMainMenu()
    {
        //Hide menu stuff
        for (int i = 0; i < menuItems.Length; i++)
        {
            menuItems[i].SetActive(true);
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
