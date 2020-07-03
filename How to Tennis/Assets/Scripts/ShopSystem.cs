using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    //Variable to store the player
    public Player player;
    //Reference to the credits text
    public TextMeshProUGUI creditsText;
    //Reference to the audio source component
    private AudioSource audioSource;
    //Reference to the purchase sfx
    public AudioClip successfulPurcahseSFX;
    //Reference to the info box script
    public InfoBox infoBox;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("ERROR: Unable to get audio source");
        }
        else
        {
            audioSource.clip = successfulPurcahseSFX;
        }
    }

    /// <summary>
    /// Function that attempts to purchase an item for the player
    /// </summary>
    public void buyItem(SkinObject item, int cost)
    {
        //Check to see if the player can afford the item
        if (player.credits >= cost)
        {
            //Player can afford the item. Allow purchase
            player.skin = item;
            player.addSkin(item.ID);
            player.credits -= cost;
            //Update the credits text
            creditsText.text = "Credits: " + player.credits;
            SaveSystem.SavePlayer(player);
            //Play a noise
            audioSource.Play();
            //Refresh the UI
            infoBox.refreshData();
        }
        else
        {
            //Player can't afford the item. Don't allow purchase
        }
    }
}
