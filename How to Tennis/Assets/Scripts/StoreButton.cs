//This script is never used???

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyButton : MonoBehaviour
{
    public SkinObject linkedSkin;
    public ShopSystem shopSystem;
    public void run()
    {
        //Call the shop system purchase function
        shopSystem.buyItem(linkedSkin, linkedSkin.cost);
    }
}
