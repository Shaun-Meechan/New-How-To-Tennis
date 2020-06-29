using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButton : MonoBehaviour
{
    //Variable to store a skin
    public SkinObject linkedSkin;
    //Variable to hold our store system
    public ShopSystem shopSystem;

    /// <summary>
    /// Function to call the shop system purchase function
    /// </summary>
    public void run()
    {
        shopSystem.buyItem(linkedSkin, linkedSkin.cost);
    }
}
