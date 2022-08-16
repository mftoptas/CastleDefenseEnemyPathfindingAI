using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[ExecuteAlways] //Makes instances of a script always execute, both as part of Play Mode and when editing.
[RequireComponent(typeof(TextMeshPro))]

public class CoordinateLabeler : MonoBehaviour
{
    [SerializeField] Color defaultColor = Color.white;
    [SerializeField] Color blockedColor = Color.gray;
    [SerializeField] Color exploredColor = Color.yellow;
    [SerializeField] Color pathColor = new Color(1f, 0.5f, 0f);

    TextMeshPro label;
    Vector2Int coordinates = new Vector2Int();
    GridManager gridManager;

    void Awake() // Awake method gets called before start.
    {
        gridManager = FindObjectOfType<GridManager>();
        label = GetComponent<TextMeshPro>();
        label.enabled = false;

        DisplayCoordinates(); // Now coordinates looks after game starts as well.
    }

    void Update()
    {
        if(!Application.isPlaying)
        {
            DisplayCoordinates();
            UpdateObjectName();
            label.enabled = true;
        }

        SetLabelColor();
        ToggleLabels();
    }

    void ToggleLabels()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            label.enabled = !label.IsActive(); // If C key is pressed labels are not going to appear.
        }
    }

    void SetLabelColor()
    {
        if(gridManager == null) { return; }

        Node node = gridManager.GetNode(coordinates);

        if(node == null) { return; } // If the first if statement returns null it breaks the third if statement. To prevent this we write this if statement.

        if(!node.isWalkable)
        {
            label.color = blockedColor;
        }
        else if(node.isPath)
        {
            label.color = pathColor;
        }
        else if(node.isExplored)
        {
            label.color = exploredColor;
        }
        else
        {
            label.color = defaultColor;
        }        
    }

    void DisplayCoordinates()
    {
        if(gridManager == null) { return; }

        coordinates.x = Mathf.RoundToInt(transform.parent.position.x / gridManager.UnityGridSize);
        coordinates.y = Mathf.RoundToInt(transform.parent.position.z / gridManager.UnityGridSize);
        
        label.text = coordinates.x + "," + coordinates.y;
    }
    
    void UpdateObjectName()
    {
        transform.parent.name = coordinates.ToString();
    }
}