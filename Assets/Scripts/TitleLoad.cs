using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleLoad : MonoBehaviour
{
    
    private bool isLoading;

    private void Start()
    {
        MusicPlayer.Instance.PlayIntro();
    }

    void Update()
    {
        if (isLoading)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            isLoading = true;
            MusicPlayer.Instance.PlayTransition();
            SceneManager.LoadScene("Game");
        }
    }
}
