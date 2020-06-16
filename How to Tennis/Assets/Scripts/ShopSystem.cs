using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSystem : MonoBehaviour
{
    public Player player;
    public void buyItem(SkinObject item, int cost)
    {
        if (player.credits >= cost)
        {
            //Player can afford the item. Allow purchase
            player.skin = item;
            player.credits -= cost;
            //Play a noise
        }
        else
        {
            //Player can't afford the item. Don't allow purchase
            //Play a noise and show an icon or something
        }
    }
}
