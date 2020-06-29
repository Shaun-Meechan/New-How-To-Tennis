using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioToggle : MonoBehaviour
{
    //Store a bool to decid if we play audio or not
    private bool playing = true;
    //Create an array to store the sprites used to represent mute and un-mute
    public Sprite[] sprites;
    //Store a reference to the current image we are displaying.
    private Image image;
    //Variable to store our audio mixer.
    public AudioMixer audioMixer;

    private void Start()
    {
        //Get our image sprite and store it.
        image = GetComponent<Image>();
    }

    /// <summary>
    /// Switches the state of audio.
    /// </summary>
    public void toggle()
    {
        switch (playing)
        {
            //If we are playing audio, stop
            case true:
                playing = false;
                //Change Icon
                image.sprite = sprites[1];
                audioMixer.SetFloat("Volume", -80f);
                break;
            //if we are not playing audio, start
            case false:
                playing = true;
                //Change Icon
                image.sprite = sprites[0];
                audioMixer.SetFloat("Volume", 0f);
                break;
        }
    }
}
