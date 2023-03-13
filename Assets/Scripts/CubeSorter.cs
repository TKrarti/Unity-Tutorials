using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteInEditMode]
public class CubeSorter : MonoBehaviour
{
    List<string> blockNames = new List<string>();
    
    private void Awake()
    {
        SortHierarchy();
    }

    void SortHierarchy()
    {
        CubeEditor[] blocks = FindObjectsOfType<CubeEditor>();
        foreach(CubeEditor block in blocks)
        {
            this.blockNames.Add(block.name);
        }
        string[] blockNames = this.blockNames.ToArray();

        IComparer comparer = new AlphaNumComparator();
        Array.Sort(blockNames, blocks, comparer); 

        for(int index = 0; index < blocks.Length; index++)
        {
            blocks[index].SetIndex(index);
        }
    }
}
