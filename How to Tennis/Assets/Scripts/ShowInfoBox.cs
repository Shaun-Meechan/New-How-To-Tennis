using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowInfoBox : MonoBehaviour
{
    public SkinObject linkedSkin;
    public InfoBox infoBox;

    public void run()
    {
        infoBox.setSkinObject(linkedSkin);
        infoBox.gameObject.SetActive(true);
    }
}
