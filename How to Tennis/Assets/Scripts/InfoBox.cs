using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class InfoBox : MonoBehaviour
{
    private SkinObject skinObject;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;
    public Player player;
    public GameObject buyButton;
    public BuyButton buyButtonScript;
    public GameObject selectButton;
    public void setSkinObject(SkinObject newSkinObject)
    {
        skinObject = newSkinObject;
        buyButtonScript.linkedSkin = newSkinObject;
        setupData();
    }

    private void setupData()
    {
        bool found = false;
        descriptionText.text = skinObject.description;
        costText.text = "Price: " + skinObject.cost;

        //make sure i is less than the total number of skins
        for (int i = 0; i < 4; i++)
        {
            if (player.skinsOwnedIDs[i] == skinObject.ID)
            {
                //The player owns this skin so show the select button not the buy button
                found = true;
            }
        }

        if (found == true)
        {
            buyButton.SetActive(false);
            selectButton.SetActive(true);
        }
        else
        {
            selectButton.SetActive(false);
            buyButton.SetActive(true);
        }
    }
}
