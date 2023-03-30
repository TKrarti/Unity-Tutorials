using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[SelectionBase]
public class CubeEditor : MonoBehaviour
{
    Waypoint waypoint;
    int gridSize;

    private void Awake()
    {
        waypoint = GetComponent<Waypoint>();
        if (waypoint)
        {
            gridSize = waypoint.GetGridSize();
        }
        else
        {
            gridSize = 10;
        }
    }

    void Update()
    {
        SnapToGrid();
        UpdateLabel();
    }

    private void SnapToGrid()
    {
        transform.position = new Vector3(
            GetGridPos().x * gridSize, 
            0, 
            GetGridPos().y * gridSize);
    }

    private void UpdateLabel()
    {
        TextMesh textMesh = GetComponentInChildren<TextMesh>();
        if(textMesh)
        {
            string labelText =
            GetGridPos().x + "," +
            GetGridPos().y;
            gameObject.name = labelText;
            textMesh.text = labelText;
        }
    }

    public void SetIndex(int pos)
    {
        transform.SetSiblingIndex(pos);
    }

    private Vector2Int GetGridPos()
    {
        return new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.z / gridSize));
    }
}
