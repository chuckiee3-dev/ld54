using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public enum MusicToPlay
    {
        None,
        Intro,
        Transition,
        Main
    }
    public AudioClip introLoop;
    public AudioClip transition;
    public AudioClip mainLoop;
    public AudioSource source;
    private MusicToPlay musicToPlay = MusicToPlay.Intro;
    private MusicToPlay currentlyPlaying = MusicToPlay.None;
    public static MusicPlayer Instance { get; private set; }
    private void Awake() 
    { 
        // If there is an instance, and it's not me, delete myself.
    
        if (Instance != null && Instance != this) 
        { 
            Destroy(this); 
        } 
        else 
        { 
            Instance = this; 
        } 
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (currentlyPlaying != musicToPlay)
        {
            source.loop = false;
            if (!source.isPlaying)
            {
                source.clip = GetClip(musicToPlay);
                source.Play();
            }
        }
    }

    private AudioClip GetClip(MusicToPlay music)
    {
        switch (music)
        {
            case MusicToPlay.Intro:
                source.loop = true;
                currentlyPlaying = MusicToPlay.Intro;
                return introLoop;
            case MusicToPlay.Transition:
                currentlyPlaying = MusicToPlay.Transition;
                return transition;
            default:
                source.loop = true;
                currentlyPlaying = MusicToPlay.Main;
                return mainLoop;
        }
    }

    public void PlayTransition()
    {
        musicToPlay = MusicToPlay.Transition;
        UniTask.Delay(TimeSpan.FromSeconds(5)).ContinueWith((PlayMainLoop));
    }
    public void PlayIntro()
    {
        musicToPlay = MusicToPlay.Intro;
    }
    public void PlayMainLoop()
    {
        musicToPlay = MusicToPlay.Main;
    }
}
