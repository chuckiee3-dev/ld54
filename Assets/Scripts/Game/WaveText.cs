using TMPro;
using UnityEngine;
using VContainer;

public class WaveText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveTmp;
    private IGameEvents gameEvents;
    private string prefix = "Wave\n";
    [Inject]
    public void Construct(IGameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
        gameEvents.OnWaveCompleted = UpdateText;
        gameEvents.OnGameStart = GameStarted;
    }

    private void GameStarted()
    {
        UpdateText(1);
    }

    private void UpdateText(int wave)
    {
        waveTmp.text = prefix + wave;
    }

    private void OnDisable()
    {
        if (gameEvents != null)
        {
            gameEvents.OnWaveCompleted -= UpdateText;
        }
    }
}
