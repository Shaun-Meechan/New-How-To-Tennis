//Script from https://www.youtube.com/watch?v=XOjd_qU2Ido
//Modified by Shaun Meechan to include relevant project data

[System.Serializable]
public class PlayerData
{
    //Variable to store how much money the player has
    public int credits;
    //Variable to store the current skin's ID
    public int skinID;
    //Variable to store all owned skins in a serializable format
    public int[] skinsOwnedIDs;
    //Variable to store if this is the player's first time playing the game
    public bool firstTime;
    //Variable to store if the user want audio muted
    public bool playAudio;

    /// <summary>
    /// Constructor for player data
    /// </summary>
    public PlayerData (Player player)
    {
        credits = player.credits;
        skinID = player.skin.ID;
        skinsOwnedIDs = player.skinsOwnedIDs;
        firstTime = player.firstTime;
        playAudio = player.playAudio;
    }
}
