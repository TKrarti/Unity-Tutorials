using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public BlockManager blockManager;
    public ParticleSystem particles;

    private void Awake()
    {
        if(particles)
        {
            particles.Stop();
        }
        blockManager.SetActiveBlock(this, transform.position);
    }

    public void OnDestroy()
    {
        blockManager.RemoveBlock(this, transform.position);
    }
}
