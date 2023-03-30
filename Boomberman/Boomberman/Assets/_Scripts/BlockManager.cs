using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class BlockManager : ScriptableObject
{
    public List<Block> activeBlocks = new List<Block>();
    public GameObject breakableBlockPrefab;
    public Dictionary<Vector3, Block> activeBlocksDictionary = new Dictionary<Vector3, Block>();
    public Vector2 maxBlock = new Vector3(5, 6);
    public Vector2 minBlock = new Vector3(-4, -3);

    public void SetActiveBlock(Block block, Vector3 position)
    {
        if(!activeBlocksDictionary.ContainsKey(position))
        {
            activeBlocks.Add(block);
            activeBlocksDictionary.Add(position, block);
        }
    }

    public void RemoveBlock(Block block, Vector3 position)
    {
        if (activeBlocksDictionary.ContainsKey(position))
        {
            activeBlocks.Remove(block);
            activeBlocksDictionary.Remove(position);
        }
    }
    
    public float breakableBlockCount()
    {
        float count = 0f;
        for(int i = 0; i < activeBlocks.Count; i++)
        {
            if(activeBlocks[i] != null && activeBlocks[i].tag == "BreakableBlock")
            {
                count++;
            }
        }
        return count;
    }
}
