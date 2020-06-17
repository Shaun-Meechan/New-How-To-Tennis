using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButton : MonoBehaviour
{
    public SkinObject linkedSkin;
    public ShopSystem shopSystem;
    public void run()
    {
        //Call the shop system purchase function
        shopSystem.buyItem(linkedSkin, linkedSkin.cost);
    }
}
