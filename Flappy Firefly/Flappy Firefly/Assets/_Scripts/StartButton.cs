using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public FloatVariable gameTime;
    public AudioSource audioSource;
    public AudioClip gameMusic;

    private void Awake()
    {
        Time.timeScale = 0;
    }

    public void StartGame()
    {
        transform.localScale = Vector3.zero;
        Time.timeScale = 1f;
        gameTime.value = 0;

        if(audioSource && gameMusic)
        {
            audioSource.clip = gameMusic;
            audioSource.loop = true;
            audioSource.Play();
        }
    }
}
