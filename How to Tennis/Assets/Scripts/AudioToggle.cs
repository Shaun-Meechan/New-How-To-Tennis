using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioToggle : MonoBehaviour
{
    private bool playing = true;
    public Sprite[] sprites;
    private Image image;
    public AudioMixer audioMixer;

    private void Start()
    {
        image = GetComponent<Image>();
    }

    public void toggle()
    {
        switch (playing)
        {
            case true:
                playing = false;
                //Change Icon
                image.sprite = sprites[1];
                audioMixer.SetFloat("Volume", -80f);
                break;
            case false:
                playing = true;
                //Change Icon
                image.sprite = sprites[0];
                audioMixer.SetFloat("Volume", 0f);
                break;
        }
    }
}
