using System;
using UnityEngine;

public class Attackable : MonoBehaviour
{
    public int health;
   
    public void Damage(int enemyDataDamageAmount)
    {
        health = Math.Max(0, health - enemyDataDamageAmount);
        if (health == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
}
