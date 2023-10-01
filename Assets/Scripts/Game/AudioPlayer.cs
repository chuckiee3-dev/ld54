using UnityEngine;
using VContainer;

public class AudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip correctChar;
    [SerializeField] private AudioClip wrongChar;
    [SerializeField] private AudioClip deleteChar;
    [SerializeField] private AudioClip winWave;
    [SerializeField] private AudioClip gainSpace;
    [SerializeField]private AudioSource[] sources;
    private IInputEvents inputEvents;
    private IBaseEvents baseEvents;
    private IGameEvents gameEvents;

    [Inject]
    public void Construct(IInputEvents inputEvents, IBaseEvents baseEvents,IGameEvents gameEvents)
    {
        this.inputEvents = inputEvents;
        this.baseEvents = baseEvents;
        this.gameEvents = gameEvents;
        this.inputEvents.OnWrongCharacter += PlayWrong;
        this.inputEvents.OnCorrectCharacter += PlayCorrect;
        this.inputEvents.OnDeleteCharacter += PlayDelete;
        this.baseEvents.OnSpaceEarned += PlaySpace;
        this.gameEvents.OnWaveCompleted += PlayWave;
    }

    private void PlayWave(int wave)
    {
        PlayClip(winWave);
    }

    private void PlaySpace(int amount)
    {
        PlayClip(gainSpace);
    }

    private void Start()
    {
        foreach (var s in sources)
        {
            s.loop = false;
            s.playOnAwake = false;
        }
    }

    private void PlayDelete()
    {
        PlayClip(deleteChar);
    }

    private void PlayClip(AudioClip audioClip)
    {
        foreach (var source in sources)
        {
            if (!source.isPlaying)
            {
                source.clip = audioClip;
                source.Play();
                break;
            }
        }
    }

    private void PlayCorrect()
    {
       PlayClip(correctChar);
    }

    private void PlayWrong()
    {
       PlayClip(wrongChar);
    }
}
