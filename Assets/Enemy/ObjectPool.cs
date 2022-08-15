using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] [Range(0, 50)] int poolSize = 5;
    [SerializeField] [Range(0.1f, 30f)] float spawnTimer = 2.5f; // I want spawnTimer not to be negative or equal to 0 and not to be too much long.

    GameObject[] pool;

    void Awake() // I like to do things in awake so that if any other scripts are going to need these objects later on, hopefully everything will be ready by the time they try and grab them.
    {
        PopulatePool();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemy());
    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];

        for(int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemyPrefab, transform); // The first thing we want to do is to instantiate our object and place it in the array. Instantiate(enemyPrefab, transform) = Instantiate the enemy prefab. And the parent is transform, which is our object pull in our hierarchy.
            pool[i].SetActive(false); // Disable this game object by default.
        }
    }

    void EnableObjectInPool()
    {
        for(int i = 0; i < pool.Length; i++)
        {
            if(pool[i].activeInHierarchy == false)
            {
                pool[i].SetActive(true);
                return;
            }
        }
    }

    IEnumerator SpawnEnemy()
    {
        while(true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(spawnTimer);
        }
    }
}