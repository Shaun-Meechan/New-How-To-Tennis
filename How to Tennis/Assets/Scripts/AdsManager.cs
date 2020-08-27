﻿using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    //Create a variable storing our gameID depending on the platform.
    //These IDs are from the Unity dashboard.
#if UNITY_IOS
    private string gameID = "3509088";
#elif UNITY_ANDROID
    private string gameID = "3509089";
#elif UNITY_STANDALONE
    private string gameID = "0000000";
#endif

    //myPlacementID should be equal to the type of ad we want to show.
    private string myplacementID = "rewardedVideo";
    //Get a reference to our player to give them credits.
    private Player player;
    //Get a reference to our credits text so we can update it.
    public TextMeshProUGUI creditsText;
    //Get a reference to our button so we know when to show an ad.
    private Button adButton;

    //Get a reference to our error message object
    public GameObject errorMessageObject;
    //Bool to see if we should show an ad
    private bool allowAd = true;
    void Start()
    {
        adButton = GameObject.FindGameObjectWithTag("ad button").GetComponent<Button>();
        player = (Player)FindObjectOfType(typeof(Player));
        //Make sure the button can't be interacted with until there is an ad to show.
        adButton.interactable = Advertisement.IsReady(myplacementID);
        //Create a listener for the ads service and initialise the service.
        Advertisement.AddListener(this);
        Advertisement.Initialize(gameID, true);
    }

    public void showAd()
    {
        Advertisement.Show(myplacementID);
    }

    public void OnUnityAdsDidError(string message)
    {
        //Failed to show ad, show error message.
        Debug.LogError("ERROR: Unable to show ad");
        Debug.LogError("Ad debug data:" + message);
        errorMessageObject.SetActive(true);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        //If the ad finished reward the player.
        if (showResult == ShowResult.Finished)
        {
            //We have to do this to fix a very annoying problem!
            //If the user does not watch an ad the first time but try again after the next match
            //Unity will not be able to find this button so can't turn it off or stop more ad's being played
            //No idea why this happens!!
            if (adButton == null)
            {
                adButton = adButton = GameObject.FindGameObjectWithTag("ad button").GetComponent<Button>();
            }
            
            player.credits += 15;
            SaveSystem.SavePlayer(player);
            creditsText.text = "You have " + player.credits + " credits";


            adButton.interactable = false;
            allowAd = false;
        }
        //If the ad failed don't reward the player and show an error message.
        else if (showResult == ShowResult.Failed)
        {
            Debug.LogError("ERROR: The ad did not finish due to an error.");
            errorMessageObject.SetActive(true);
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //Function not used but required to compile.
    }

    public void OnUnityAdsReady(string placementId)
    {
        //If the ready placement is ready allow the user to press the button.
        if (placementId == myplacementID && allowAd == true)
        {
            adButton.interactable = true;
        }
    }

}
