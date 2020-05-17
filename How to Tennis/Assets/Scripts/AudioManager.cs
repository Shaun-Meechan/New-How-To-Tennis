using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] scoreClips;
    public AudioClip[] hitClips;
    public AudioClip applause;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playScoreClip(int ID)
    {
        audioSource.clip = scoreClips[ID];
        audioSource.Play();
    }

    public void playHitClip(int ID)
    {
        audioSource.clip = hitClips[ID];
        audioSource.Play();
    }

    public void playApplause()
    {
        audioSource.clip = applause;
        audioSource.Play();
    }

}
