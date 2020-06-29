using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //Reference to an audio source
    private AudioSource audioSource;
    //Create an array of clips to play when the ball is hit
    public AudioClip[] hitClips;
    //Reference to the clip to play when someone wins.
    public AudioClip applause;
    void Start()
    {
        //Get the audio source and store it.
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Function to play an exact clip
    /// </summary>
    /// <param name="ID">ID of the clip stored in the array.</param>
    public void playHitClip(int ID)
    {
        //Check to see if the requested clip exists. If it doesn't then debug an error otherwise play it.
        if (ID > hitClips.Length)
        {
            Debug.LogError("ERROR: Unable to play hit clip with ID " + ID + " as ID is greater than the size of the array. Has the clip been added to the array?");
        }
        else
        {
            audioSource.clip = hitClips[ID];
            audioSource.Play();
        }
    }

    /// <summary>
    /// Play a random hit clip
    /// </summary>
    public void playRandomHitClip()
    {
        //Create a int to store the ID of the random clip
        int ID = Random.Range(0, hitClips.Length);

        audioSource.clip = hitClips[ID];
        audioSource.Play();
    }

    /// <summary>
    /// Play the applause clips
    /// </summary>
    public void playApplause()
    {
        audioSource.clip = applause;
        audioSource.Play();
    }

}
