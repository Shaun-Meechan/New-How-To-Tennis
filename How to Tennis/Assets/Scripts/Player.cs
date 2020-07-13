//Script from https://www.youtube.com/watch?v=XOjd_qU2Ido
//Modified by Shaun Meechan to include relevan project data.
using UnityEngine;

public class Player : MonoBehaviour
{
    //Variable to represent how much money the player has
    public int credits = 0;
    //Array to store what skins the player has in a Serializable format
    public int[] skinsOwnedIDs;
    //Variable storing the current skin being used
    public SkinObject skin;
    //Variable to represent if this is the player's first time playing the game
    public bool firstTime = true;
    //Variable to store if the user want audio muted
    public bool playAudio = true;

    /// <summary>
    /// Function to add a new skin to the player owned skins
    /// </summary>
    public void addSkin(int ID)
    {
        Debug.Log("Adding a skin to array");
        for (int i = 0; i < 4; i++)
        {
            if (i == 0)
            {
                //Always want element 1 to be 0 so do nothing
            }
            else if (skinsOwnedIDs[i] == 0)
            {
                //Overwrite the 0
                Debug.Log("Added a skins to the array!");
                skinsOwnedIDs[i] = ID;
                return;
            }
        }
    }

    /// <summary>
    /// Function to reset the player's skin array.
    /// SHOULD ONLY BE USED IF THE SAVE FILE HAS BEEN CORRUPTED IN SOME WAY!
    /// </summary>
    public void resetSkinsArray()
    {
        for (int i = 0; i < skinsOwnedIDs.Length; i++)
        {
            skinsOwnedIDs[i] = 0;
        }
    }
    /// <summary>
    /// Function to set the value of playAudio.
    /// </summary>
    public void setPlayAudio(bool value)
    {
        playAudio = value;
    }

    /// <summary>
    /// Returns the value of playAudio.
    /// </summary>
    public bool getPlayAudio()
    {
        return playAudio;
    }
}
