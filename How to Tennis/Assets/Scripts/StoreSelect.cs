using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreSelect : MonoBehaviour
{
    //Variable to store a skin
    public SkinObject linkedSkin;
    //Variable to hold our store system
    public ShopSystem shopSystem;
    //Variable to store the player
    public Player player;


    /// <summary>
    /// Function to call the shop system purchase function
    /// </summary>
    public void run()
    {
        player.skin = linkedSkin;
        player.skinID = linkedSkin.ID;
        SaveSystem.SavePlayer(player);
    }

}
