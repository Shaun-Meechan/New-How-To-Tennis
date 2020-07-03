using UnityEngine;
using TMPro;
public class InfoBox : MonoBehaviour
{
    //Variable to store a passed in skin object
    private SkinObject skinObject;
    //Variable to store the description text of the passed in skin
    public TextMeshProUGUI descriptionText;
    //Variable to store the cost text of the passed in skin
    public TextMeshProUGUI costText;
    //Variable to store the player
    public Player player;
    //Variable to store the buy button
    public GameObject buyButton;
    //Variable to store the buy button script
    public BuyButton buyButtonScript;
    //Variable to store the select button
    public GameObject selectButton;

    /// <summary>
    /// Function to set our local skin object to a skin that has been passed in
    /// </summary>
    public void setSkinObject(SkinObject newSkinObject)
    {
        skinObject = newSkinObject;
        buyButtonScript.linkedSkin = newSkinObject;
        setupData();
    }

    /// <summary>
    /// Function to set the text to the correct input and find out if the player can select the skin or they need to buy iy
    /// </summary>
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

    /// <summary>
    /// Function to be used after the player purchases an item
    /// </summary>
    public void refreshData()
    {
        setupData();
    }
}
