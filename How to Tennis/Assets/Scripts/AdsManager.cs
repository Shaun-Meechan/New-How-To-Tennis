using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using TMPro;
using UnityEngine.UI;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    private string placement = "reward video";
    private Player player;
    public TextMeshProUGUI creditsText;
    public Button adButton;
    void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize("3509089", true);
    }

    public void showAd(string p)
    {
        Advertisement.Show(p);
    }
    public void OnUnityAdsDidError(string message)
    {
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            //player.credits += 15;
            //SaveSystem.SavePlayer(player);
            //creditsText.text = "You have " + player.credits + " credits";
            adButton.interactable = false;
        }
        else if (showResult == ShowResult.Failed)
        {
            //Failed to show ad, do nothing
        }
    }

    public void OnUnityAdsDidStart(string placementId)
    {
    }

    public void OnUnityAdsReady(string placementId)
    {
    }

}
