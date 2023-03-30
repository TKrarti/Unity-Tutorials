using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] Waypoint end;

    Waypoint start;
    Dictionary<Vector2Int, Waypoint> grid = new Dictionary<Vector2Int, Waypoint>();
    Queue<Waypoint> queue = new Queue<Waypoint>(); 
    Waypoint searchCenter;
    List<Waypoint> path = new List<Waypoint>();
    bool pathBlocked = false;

    Vector2Int[] directions = {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left,
    };

    public List<Waypoint> GetPath()
    {
        ResetPath();
        DetermineEnd();
        LoadWaypoints();
        BreadthFirstSearch();
        DetermineIfPathBlocked();
        if(!pathBlocked)
        {
            CreatePath();
        }
        return path;
    }

    private void ResetPath()
    {
        pathBlocked = false;
        queue.Clear();
        path.Clear();
        grid.Clear();
    }
    private void DetermineEnd()
    {
        var enemySpawner = GetComponentInParent<EnemySpawner>();
        if (enemySpawner)
        {
            end = enemySpawner.GetEnd();
        }
    }

    private void CreatePath()
    {
        path.Add(end);
        if (start != end)
        {
            Waypoint previous = end.exploredFrom;
            while (previous != start)
            {
                path.Add(previous);
                previous = previous.exploredFrom;
            }
            path.Add(start);
            path.Reverse();
        }
    }

    private void BreadthFirstSearch()
    {
        queue.Enqueue(start);
        while (queue.Count > 0)
        {
            searchCenter = queue.Dequeue();
            searchCenter.isExplored = true;
            if (searchCenter == end)
            {
                break;
            }
            ExploreNeighbors();
        }
    }

    private void DetermineIfPathBlocked()
    {
        if (queue.Count == 0 && searchCenter != end)
        {
            pathBlocked = true;
        }
    }

    private void ExploreNeighbors()
    {
        foreach (Vector2Int direction in directions)
        {
            Vector2Int neighborCoordinates = searchCenter.GetGridPos() + direction;
            
            if(grid.ContainsKey(neighborCoordinates))
            {
                QueueNewNeighbors(neighborCoordinates);
            }
        }
    }

    private void QueueNewNeighbors(Vector2Int neighborCoordinates)
    {
        Waypoint neighbor = grid[neighborCoordinates];
        if(!neighbor.isExplored && !queue.Contains(neighbor))
        {
            queue.Enqueue(neighbor);
            neighbor.exploredFrom = searchCenter;
        }
    }

    private void LoadWaypoints()
    {
        var waypoints = FindObjectsOfType<Waypoint>();

        Vector2Int currentPos = new Vector2Int(
            (int)(transform.position.x / waypoints[1].GetGridSize()),
            (int)(transform.position.z / waypoints[1].GetGridSize())); 

        foreach (Waypoint waypoint in waypoints)
        {
            waypoint.exploredFrom = null;
            waypoint.isExplored = false;
            bool isOverlapping = grid.ContainsKey(waypoint.GetGridPos());
            if (isOverlapping)
            {
                Debug.LogWarning("Skipping overlapping block " + waypoint);
            }
            else if(waypoint.isExplorable)
            {
                grid.Add(waypoint.GetGridPos(), waypoint);
            }

            if(waypoint.GetGridPos() == currentPos)
            {
                start = waypoint;
            } 
        }
    }
}
