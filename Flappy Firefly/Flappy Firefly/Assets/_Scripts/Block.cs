using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Block : MonoBehaviour
{
    Transform blockGenerator;
    int blockNumber;
    bool smallBlock;

    public void SetBlockNumberAndSpawn(int _blockNumber, Transform _blockGenerator, bool _smallBlock)
    {
        blockNumber = _blockNumber;
        blockGenerator = _blockGenerator;
        smallBlock = _smallBlock;

        Vector3 pos = Vector3.zero;
        pos.x = Camera.main.transform.position.x + 1.25f + blockNumber;

        PlaceBlock(pos);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.tag == "Cleaner")
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 pos = transform.position;
        pos.x = blockGenerator.position.x;
        PlaceBlock(pos);
    }

    private void PlaceBlock(Vector3 pos)
    {
        if (smallBlock)
        {
            if (blockNumber % 2 == 0)
            {
                pos.y = UnityEngine.Random.Range(0.40f, 1f);
            }
            else
            {
                pos.y = UnityEngine.Random.Range(1.70f, 2.25f);
            }
        }
        else
        {
            if (blockNumber % 2 == 1)
            {
                pos.y = UnityEngine.Random.Range(0.40f, 1f);
            }
            else
            {
                pos.y = UnityEngine.Random.Range(1.70f, 2.25f);
            }
        }

        transform.position = pos;
    }
}
