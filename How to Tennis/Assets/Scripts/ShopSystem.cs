using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    //Variable to store the player
    public Player player;
    //Reference to the credits text
    public TextMeshProUGUI creditsText;

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
        }
        else
        {
            //Player can't afford the item. Don't allow purchase
            //Play a noise and show an icon or something
        }
    }
}
