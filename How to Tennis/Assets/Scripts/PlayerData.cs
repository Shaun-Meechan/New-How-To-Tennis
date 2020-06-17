using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int credits;
    public int skinID;
    public int[] skinsOwnedIDs;
    public bool firstTime;
    public PlayerData (Player player)
    {
        credits = player.credits;
        skinID = player.skin.ID;
        skinsOwnedIDs = player.skinsOwnedIDs;
        firstTime = player.firstTime;
    }
}
