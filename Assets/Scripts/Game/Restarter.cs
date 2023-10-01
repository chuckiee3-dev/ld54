using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

public class Restarter : MonoBehaviour
{
    public KeyCode restartKey;

    public CanvasGroup endGameCanvas;
    public float durationToHold;

    private float timer;
    private IGameEvents gameEvents;
    private bool isActive;
    
    [Inject]
    public void Construct(IGameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
        gameEvents.OnGameOver += Activate;
    }

    private void Activate()
    {
        if (isActive)
        {
            return;
        }
        isActive = true;
        float alpha = 0;
        DOTween.To(() => alpha, x => alpha = x, 1, .325f)
            .OnUpdate(() => { endGameCanvas.alpha = alpha; });
    }

    void Update()
    {
        if (!isActive)
        {
            return;
        }
        if (Input.GetKey(restartKey))
        {
            timer += Time.deltaTime;
        }

        if (Input.GetKeyUp(restartKey))
        {
            timer -= Time.deltaTime;
        }

        timer = Mathf.Clamp(timer, 0, durationToHold);
        if (Mathf.Abs(timer - durationToHold) <= 0.1f)
        {
            timer = 0;
            SceneManager.LoadScene("Title");
        }
    }
}
