using System.Collections;
using System.Collections.Generic;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;

public class EndScreenUISetup : MonoBehaviour
{
    public RectTransform rectTransform;
    float newWidth = (Screen.width / 3) + 12;

    void Start()
    {
        //Get the attached rect transform
        rectTransform = GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(newWidth, Screen.height);
    }

}
