using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    //Variable to store all the skins in the game
    public SkinObject[] skins;

    /// <summary>
    /// Returns a skin material based on a passed in ID
    /// </summary>
    public Material getSkinMaterial(int ID)
    {
        bool found = false;
        for (int i = 0; i < skins.Length; i++)
        {
            //Look through all the skins to make sure the ID requested exists.
            if (ID == skins[ID].ID)
            {
                //We found the skins we want. Set found to true and break.
                found = true;
                break;
            }
        }

        if (found == true)
        {
            return skins[ID].skin;
        }
        else
        {
            Debug.LogError("ERROR: Skin loader unable to find skin with ID" + ID + ". Has it been added to the object's array?");
            return null;
        }
    }

    /// <summary>
    /// Returns a skin based on the passed in ID
    /// </summary>
    public SkinObject getSkin(int ID)
    {
        bool found = false;
        for (int i = 0; i < skins.Length; i++)
        {
            //Look through all the skins to make sure the ID requested exists.
            if (ID == skins[ID].ID)
            {
                //We found the skins we want. Set found to true and break.
                found = true;
                break;
            }
        }

        if (found == true)
        {
            return skins[ID];
        }
        else
        {
            Debug.LogError("ERROR: Skin loader unable to find skin with ID" + ID + ". Has it been added to the object's array?");
            return null;
        }

    }
}
