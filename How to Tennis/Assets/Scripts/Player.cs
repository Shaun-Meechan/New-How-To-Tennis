using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int credits = 0;
    public int[] skinsOwnedIDs;
    public SkinObject skin;
    public bool firstTime = true;
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

    public void resetSkinsArray()
    {
        for (int i = 0; i < skinsOwnedIDs.Length; i++)
        {
            skinsOwnedIDs[i] = 0;
        }
    }
}
