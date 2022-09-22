using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int columnLength, rowLength;
    public int blockWidth;
    [Serializable]
    public struct MazeBlock
    {
        public int quantity;
        public GameObject prefab;
    }
    public MazeBlock[] mazeBlocksToGenerate;
    public GameObject[] placementBlocks;
    Transform[,] mazeBlocks;
    public float moveTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        mazeBlocks = new Transform[rowLength, columnLength];
        //create array with all the blocks we want to use
        var allMazeBlocks = new GameObject[columnLength * rowLength];
        int idx = 0;
        foreach (MazeBlock mazeBlock in mazeBlocksToGenerate)
        {
            for (int i = 0; i < mazeBlock.quantity; i++)
            {
                if (idx < columnLength * rowLength)
                {
                    allMazeBlocks[idx] = mazeBlock.prefab;
                    idx++;
                }
                else
                    Debug.LogWarning("number of maze blocks is bigger than cell amount, please fix!");
            }
        }
        if (idx < columnLength * rowLength)
            Debug.LogWarning("number of maze blocks is smaller than cell amount, please fix!");


        //randomize array
        for (int i = 0; i < allMazeBlocks.Length - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, allMazeBlocks.Length);
            var temp = allMazeBlocks[rnd];
            allMazeBlocks[rnd] = allMazeBlocks[i];
            allMazeBlocks[i] = temp;
        }

        //place blocks on grid
        for (int i = 0; i < columnLength * rowLength; i++)
        {
            GameObject block = Instantiate(allMazeBlocks[i], new Vector3(i / columnLength * blockWidth, 0, i % columnLength * blockWidth), Quaternion.identity);
            mazeBlocks[i / columnLength, i % columnLength] = block.transform;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0))
        {
            //only for testing, needs to be improved once we know how we do the interaction (table/tablet..)
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = UnityEngine.Camera.main.transform.position.y;
            Vector3 pos = UnityEngine.Camera.main.ScreenToWorldPoint(screenPos);
            var column = (int)((pos.x + blockWidth / 2) / blockWidth);
            var row = (int)((pos.z + blockWidth / 2) / blockWidth);
            var newBlockIdx = UnityEngine.Random.Range(0, placementBlocks.Length ); //should be selected by the user in the future
            if (pos.x + blockWidth / 2 < 0 && row < columnLength)//move row right
                moveInPositiveDir(true, row, newBlockIdx);
            else if ((pos.x + blockWidth / 2) / blockWidth > rowLength && row < columnLength) //move row left
                moveInNegativeDir(true, row, newBlockIdx);
            else if (pos.z + blockWidth / 2 < 0 && column < rowLength)//move column up
                moveInPositiveDir(false, column, newBlockIdx);
            else if (((pos.z + blockWidth / 2) / blockWidth > columnLength) && (column < rowLength))    //move column down
                moveInNegativeDir(false, column, newBlockIdx);
        }
    }
    void moveInPositiveDir(bool isRow, int idx, int newBlockId)//right or up
    {
        for (int i = (isRow ? rowLength : columnLength) - 1; i >= 0; i--)
        {
            var block = isRow ? mazeBlocks[i, idx] : mazeBlocks[idx, i];
            var moveVector = isRow ? new Vector3(blockWidth, 0, 0) : new Vector3(0, 0, blockWidth);
            var tween = block.DOMove(block.position + moveVector, moveTime);
            if (isLastBlock(isRow, i, false))//delete last block
                tween.OnComplete(() => Destroy(block.gameObject));
            else
            {
                if (isRow) mazeBlocks[i + 1, idx] = block;
                else mazeBlocks[idx, i + 1] = block;
            }
        }
        placeNewBlock(idx, isRow, false, newBlockId);

    }
    void moveInNegativeDir(bool isRow, int idx, int newBlockId)//left or down
    {
        for (int i = 0; i < (isRow ? rowLength : columnLength); i++)
        {
            var block = isRow ? mazeBlocks[i, idx] : mazeBlocks[idx, i];
            var moveVector = isRow ? new Vector3(-blockWidth, 0, 0) : new Vector3(0, 0, -blockWidth);
            var tween = block.DOMove(block.position + moveVector, moveTime);
            if (isLastBlock(isRow, i, true))//delete last block
                tween.OnComplete(() => Destroy(block.gameObject));
            else
            {
                if (isRow) mazeBlocks[i - 1, idx] = block;
                else mazeBlocks[idx, i - 1] = block;
            }
        }
        placeNewBlock(idx, isRow, true, newBlockId);

    }
    bool isLastBlock(bool isRow, int idx, bool moveLeft)
    {
        if (isRow) return idx == rowLength - 1 && !moveLeft || idx == 0 && moveLeft;
        else return idx == columnLength - 1 && !moveLeft || idx == 0 && moveLeft;
    }

    void placeNewBlock(int idx, bool inRow, bool moveLeft, int newBlockId)

    {
        Vector3 pos;
        if (inRow) pos = new Vector3(moveLeft ? rowLength * blockWidth : -blockWidth, 0, idx * blockWidth);
        else pos = new Vector3(idx * blockWidth, 0, moveLeft ? columnLength * blockWidth : -blockWidth);
        var newBlock = Instantiate(placementBlocks[newBlockId], pos, Quaternion.identity);
        var moveVector = inRow ? new Vector3((moveLeft ? -1 : 1) * blockWidth, 0, 0) : new Vector3(0, 0, (moveLeft ? -1 : 1) * blockWidth);
        var tween = newBlock.transform.DOMove(newBlock.transform.position + moveVector, moveTime);
        tween.OnComplete(() =>
        {
            if (inRow) mazeBlocks[moveLeft ? rowLength - 1 : 0, idx] = newBlock.transform;
            else mazeBlocks[idx, moveLeft ? columnLength - 1 : 0] = newBlock.transform;
        });
    }

}
