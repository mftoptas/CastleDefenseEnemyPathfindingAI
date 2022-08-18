using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{  
    [SerializeField] [Range(0f, 5f)] float speed = 0.5f; // Enemy speed coefficient.

    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;

    void OnEnable()
    {
        ReturnStart();
        RecalculatePath(true);
    }

    void Awake()
    {
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();
    }

    void RecalculatePath(bool resetPath)
    {
        Vector2Int coordinates = new Vector2Int();

        if(resetPath)
        {
            coordinates = pathfinder.StartCoordinates;
        }
        else
        {
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }

        StopAllCoroutines();
        path.Clear(); // So when we find a path, we're going to clear the existing one and then add a new one.
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath()); // The execution of a coroutine can be paused at any point using the yield statement. When a yield statement is used, the coroutine pauses execution and automatically resumes at the next frame.
    }

    void ReturnStart()
    {
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates); // It's going to move our enemy into the first waypoint.
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false); // Rather than destroying our game object and removing it from the pool entirely, this is going to disable it in the hierarchy and then it will be free for the pool to reuse again later.
    }

    IEnumerator FollowPath()
    {
        for(int i = 1; i < path.Count; i++) // Get started by coroutine.
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);
            float travelPercent = 0f;

            transform.LookAt(endPosition); // I am always going to be facing the waypoint that we're heading towards.

            while(travelPercent < 1f) // Means while i am not at the end position.
            {
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent); // Lerp: Linearly interpolates between two points.
                yield return new WaitForEndOfFrame();
            }
        }
        
        FinishPath();
    }
}