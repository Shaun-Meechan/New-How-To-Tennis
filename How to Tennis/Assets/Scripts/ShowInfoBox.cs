using UnityEngine;

public class ShowInfoBox : MonoBehaviour
{
    //Variable to store a linked skin
    public SkinObject linkedSkin;
    //A reference to the info box
    public InfoBox infoBox;

    /// <summary>
    /// Function to setup the data for the info box and enable it
    /// </summary>
    public void run()
    {
        infoBox.setSkinObject(linkedSkin);
        infoBox.gameObject.SetActive(true);
    }
}
