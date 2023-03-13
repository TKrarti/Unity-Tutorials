using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField] Tower towerPrefab;
    
    // public ok here as is a data class
    public bool isExplored = false;
    public Waypoint exploredFrom;
    public bool isPlaceable = true;
    public bool isExplorable = true;
    bool isOccupied = false;

    const int gridSize = 10;

    public int GetGridSize()
    {
        return gridSize;
    }

    public Vector2Int GetGridPos()
    {
        return new Vector2Int(
            Mathf.RoundToInt(transform.position.x / gridSize),
            Mathf.RoundToInt(transform.position.z / gridSize));
    }

    private void OnMouseDown()
    {
        if(towerPrefab)
        {
            DetermineIfPlaceable();
            if(isPlaceable)
            {
                isExplorable = false;
                isOccupied = true;
                var newTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
                newTower.transform.parent = GameObject.Find("Towers").transform;
            }
            else
            {
                print("not placeable");
            }
        }
    }

    private void DetermineIfPlaceable()
    {
        isExplorable = false;
        var path = GetComponentInParent<Pathfinder>().GetPath();
        print(path.Count);
        if(path.Count == 0)
        {
            isExplorable = true;
            isPlaceable = false;
        }
        else if(isOccupied)
        {
            isPlaceable = false;
        }
        else
        {
            isPlaceable = true;
        }
    }
}
