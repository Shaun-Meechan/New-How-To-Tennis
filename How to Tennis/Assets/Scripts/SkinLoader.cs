using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinLoader : MonoBehaviour
{
    public SkinObject[] skins;

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
}
