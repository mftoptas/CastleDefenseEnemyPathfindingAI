using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour
{
    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] float speed = 0.5f; // Enemy speed coefficient.

    Enemy enemy;

    void OnEnable()
    {
        FindPath();
        ReturnStart();
        StartCoroutine(FollowPath()); // The execution of a coroutine can be paused at any point using the yield statement. When a yield statement is used, the coroutine pauses execution and automatically resumes at the next frame.
    }

    void Start()
    {
        enemy = GetComponent<Enemy>();
    }

    void FindPath()
    {
        path.Clear(); // So when we find a path, we're going to clear the existing one and then add a new one.

        GameObject parent = GameObject.FindGameObjectWithTag("Path"); // This will then return the path parent object.

        foreach(Transform child in parent.transform) // What this will do now is find that parent object that we've tagged with the path and loop through all of its children in order.
        {
            Waypoint waypoint = child.GetComponent<Waypoint>();
            
            if(waypoint != null)
            {
                path.Add(waypoint); // Add waypoint to the path.
            }
        }
    }

    void ReturnStart()
    {
        transform.position = path[0].transform.position; // It's going to move our enemy into the first waypoint.
    }

    void FinishPath()
    {
        enemy.StealGold();
        gameObject.SetActive(false); // Rather than destroying our game object and removing it from the pool entirely, this is going to disable it in the hierarchy and then it will be free for the pool to reuse again later.
    }

    IEnumerator FollowPath()
    {
        foreach(Waypoint waypoint in path) // Get started by coroutine.
        {
            Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;
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