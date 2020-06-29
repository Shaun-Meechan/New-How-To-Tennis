﻿//Script from https://www.youtube.com/watch?v=YMj2qPq9CP8

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    //Variable to store the loading screen UI
    public GameObject LoadingScreen;
    //Variable to store the slider to represent the loading progress
    public Slider slider;
    //Variable to store the text representing the percentage our loading is at
    public TextMeshProUGUI percentageText;

    /// <summary>
    /// Function to start loading a new scene
    /// </summary>
    /// <param name="sceneIndex">An integer representing a scene to load</param>
    public void LoadLevel(int sceneIndex)
    {
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }

    /// <summary>
    /// Coroutine to load the new scene asynchronously
    /// </summary>
    IEnumerator LoadAsynchronously(int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            percentageText.text = progress * 100f + "%";
            yield return null;
        }
    }
}
