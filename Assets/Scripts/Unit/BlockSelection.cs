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
        UpdateSelectedBlock();
    }

    private void UpdateSelectedBlock()
    {
        foreach (BlockCoordinates bc in Blocks)
        {
            if (bc.Coordinates.SequenceEqual(Coords)) SelectedBlock = bc.Block;
        }
        SelectedBlock.transform.position = new Vector3(Coords[0], SELECTED_BLOCK_HEIGHT, Coords[1]);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) MoveSelectionLeft();
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) MoveSelectionRight();
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) MoveSelectionUp();
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) MoveSelectionDown();
    }

    private void MoveSelectionLeft()
    {
        SelectedBlock.transform.position = new Vector3(Coords[0], 0f, Coords[1]);
        Coords[0] -= 1;
        if (Coords[0] < 0) Coords[0] = 2;
        UpdateSelectedBlock();
    }

    private void MoveSelectionRight()
    {
        SelectedBlock.transform.position = new Vector3(Coords[0], 0f, Coords[1]);
        Coords[0] += 1;
        if (Coords[0] > 2) Coords[0] = 0;
        UpdateSelectedBlock();
    }

    private void MoveSelectionUp()
    {
        SelectedBlock.transform.position = new Vector3(Coords[0], 0f, Coords[1]);
        Coords[1] += 1;
        if (Coords[1] > 2) Coords[1] = 0;
        UpdateSelectedBlock();
    }

    private void MoveSelectionDown()
    {
        SelectedBlock.transform.position = new Vector3(Coords[0], 0f, Coords[1]);
        Coords[1] -= 1;
        if (Coords[1] < 0) Coords[1] = 2;
        UpdateSelectedBlock();
    }
}
