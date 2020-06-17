using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopSystem : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI creditsText;
    public void buyItem(SkinObject item, int cost)
    {
        Debug.Log("Player has attempted to buy an item!");
        if (player.credits >= cost)
        {
            //Player can afford the item. Allow purchase
            player.skin = item;
            player.addSkin(item.ID);
            player.credits -= cost;
            Debug.Log("Player purcahsed an item!");
            //Update the credits text
            creditsText.text = "Credits: " + player.credits;
            SaveSystem.SavePlayer(player);
            //Play a noise
        }
        else
        {
            //Player can't afford the item. Don't allow purchase
            //Play a noise and show an icon or something
            Debug.Log("Player can't afford item");
        }
    }
}
