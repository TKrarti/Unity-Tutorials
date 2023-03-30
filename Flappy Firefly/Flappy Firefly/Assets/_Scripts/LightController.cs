using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    Transform player;
    Transform blockGenerator;

    public FloatVariable darkness;
    int blockNumber;
    public float lightGenerated = 0.2f;
    
    public void SpawnAndSetBlockNumber(int _blockNumber, Transform _player, Transform _blockGenerator)
    {
        blockNumber = _blockNumber;
        player = _player;
        blockGenerator = _blockGenerator;

        Vector3 pos = transform.position;
        pos.x = player.position.x + 1.75f + blockNumber;
        pos.y = UnityEngine.Random.Range(0.40f, 2.00f);
        transform.position = pos;
    }

    void EnableRenderer()
    {
        GetComponent<Renderer>().enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            GetComponent<Renderer>().enabled = false;
            darkness.value -= lightGenerated;
            Invoke("EnableRenderer", 3f);
        }
        else if(collision.tag == "Cleaner")
        {
            Move();
        }
    }

    private void Move()
    {
        Vector3 pos = transform.position;
        pos.x = blockGenerator.position.x;
        pos.y = UnityEngine.Random.Range(0.40f, 2.00f);
        transform.position = pos;
    }
}
