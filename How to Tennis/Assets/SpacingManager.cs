using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpacingManager : MonoBehaviour
{
    private HorizontalLayoutGroup horizontalLayout;
    void Start()
    {
        horizontalLayout = GetComponent<HorizontalLayoutGroup>();

        horizontalLayout.spacing = Screen.width / 3;
    }
}
