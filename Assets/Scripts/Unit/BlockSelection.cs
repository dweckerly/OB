using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockSelection : MonoBehaviour
{
    const float SELECTED_BLOCK_HEIGHT = 0.3f;
    public BlockCoordinates[] Blocks;
    public GameObject SelectedBlock;
    public int[] Coords = {1, 1};
    
    void Start()
    {
        foreach (BlockCoordinates bc in Blocks)
        {
            if (bc.Coordinates.SequenceEqual(Coords)) SelectedBlock = bc.Block;
        }
        SelectedBlock.transform.position = new Vector3(Coords[0], SELECTED_BLOCK_HEIGHT, Coords[1]);
    }

    void Update()
    {
        
    }

    
}
