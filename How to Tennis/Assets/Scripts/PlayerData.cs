using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int credits;
    public int skinID;
    public PlayerData (Player player)
    {
        credits = player.credits;
        skinID = player.skin.ID;
    }
}
