using System;
using TMPro;
using UnityEngine;
using VContainer;

public class Attackable : MonoBehaviour
{
    public int health;
    public TextMeshProUGUI tmp;
    private string prefix = "Hp: ";
    [SerializeField] private AudioClip correctChar;
    [SerializeField] private AudioClip wrongChar;
    [SerializeField] private AudioClip deleteChar;
    [SerializeField]private AudioSource[] sources;
    private IGameEvents gameEvents;
    private bool isDead;
    [Inject]
    public void Construct(IGameEvents gameEvents)
    {
        this.gameEvents = gameEvents;
    }

    private void Awake()
    {
        tmp.text = prefix+health.ToString();
    }

    public void Damage(int enemyDataDamageAmount)
    {
        if (isDead)
        {
            return;
        }
        health = Math.Max(0, health - enemyDataDamageAmount);
        tmp.text = prefix+health.ToString();
        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameEvents.OnGameOver?.Invoke();
        gameObject.SetActive(false);
    }
}
