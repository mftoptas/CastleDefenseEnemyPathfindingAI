using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
// What require component does is ensures that the required component that we specify also gets attached to the game object when we attach this script.
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 3;
    [Tooltip("Adds amount to maxHitpoints when enemy dies.")]
    
    [SerializeField] int difficultyRamp = 5;

    int currentHitPoint = 0;

    Enemy enemy;

    void OnEnable()
    {
        currentHitPoint = maxHitPoints;
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoint--;

        if (currentHitPoint <= 0)
        {
            gameObject.SetActive(false); // Rather than destroying our game object and removing it from the pool entirely, this is going to disable it in the hierarchy and then it will be free for the pool to reuse again later.
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }
}