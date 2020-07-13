using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioToggle : MonoBehaviour
{
    //Create an array to store the sprites used to represent mute and un-mute
    public Sprite[] sprites;
    //Store a reference to the current image we are displaying.
    public Image image;
    //Variable to store our audio mixer.
    public AudioMixer audioMixer;
    //Variable to store our player
    public Player player;
    //Store a bool to decid if we play audio or not
    private bool playing = true;

    private void Start()
    {
        playing = player.getPlayAudio();
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
                //Disable audio
                audioMixer.SetFloat("Volume", -80f);
                //Save the players preference
                player.setPlayAudio(false);
                SaveSystem.SavePlayer(player);
                break;
            //if we are not playing audio, start
            case false:
                playing = true;
                //Change Icon
                image.sprite = sprites[0];
                //Enable audio
                audioMixer.SetFloat("Volume", 0f);
                //Save the players preference
                player.setPlayAudio(true);
                SaveSystem.SavePlayer(player);
                break;
        }
    }

    /// <summary>
    /// Mutes the audio. Intended to be used in scripts.
    /// </summary>
    public void muteAudio()
    {
        playing = false;
        //Change Icon
        image.sprite = sprites[1];
        //Disable audio
        audioMixer.SetFloat("Volume", -80f);
        //Save the players preference
        player.setPlayAudio(false);
        SaveSystem.SavePlayer(player);
    }

    /// <summary>
    /// Unmutes the audio. Intended to be used in scripts
    /// </summary>
    public void playAudio()
    {
        playing = true;
        //Change Icon
        image.sprite = sprites[0];
        //Enable audio
        audioMixer.SetFloat("Volume", 0f);
        //Save the players preference
        player.setPlayAudio(true);
        SaveSystem.SavePlayer(player);
    }
}
