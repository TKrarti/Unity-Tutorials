using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float secondsBetweenMovements = 2f;
    
    void Start()
    {
        StartCoroutine(FollowPath());
    }

    IEnumerator FollowPath()
    {
        while(true)
        {
            var path = GetComponent<Pathfinder>().GetPath();
            transform.position = path[1].transform.position;
            yield return new WaitForSeconds(secondsBetweenMovements);
            if(path.Count == 2)
            {
                Destroy(gameObject);
                break;
            }
        }
    }
}
