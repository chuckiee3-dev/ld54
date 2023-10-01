using TMPro;
using UnityEngine;
using VContainer;

public class EnemyCounter : MonoBehaviour
{
    public TextMeshProUGUI tmp;
    private IEnemyEvents enemyEvents;
    private int score = 0;
    private string prefix = "Enemies\n";
    
    [Inject]
    public void Construct(IEnemyEvents enemyEvents)
    {
        this.enemyEvents = enemyEvents;
        enemyEvents.OnEnemyDied += Increment;
    }

    private void Increment(string word)
    {
        score++;
        tmp.text = prefix + score;
    }

    private void OnDestroy()
    {
        if (enemyEvents != null)
        {
            enemyEvents.OnEnemyDied -= Increment;
        }
    }
}
