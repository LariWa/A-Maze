using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    private int columnLength, rowLength, blockWidth;   
    public GameObject[] mazeBlocksToGenerate; //index needs to match given Id in VR scene!!
    Transform[,] mazeBlocks;
    public float moveTime = 0.5f;
    public static MazeGenerator instance { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }
    public void generateMaze(int columnLength, int rowLength, int blockWidth, int[] blockIdxs)
    {
        this.columnLength = columnLength;
        this.rowLength = rowLength;
        this.blockWidth = blockWidth;
        mazeBlocks = new Transform[rowLength, columnLength];
        //place blocks on grid
        for (int i = 0; i < columnLength * rowLength; i++)
        {
            GameObject block = Instantiate(mazeBlocksToGenerate[blockIdxs[i]], new Vector3(i / columnLength * blockWidth, 0, i % columnLength * blockWidth), Quaternion.identity);
            mazeBlocks[i / columnLength, i % columnLength] = block.transform;
        }

        //place camera
        UnityEngine.Camera.main.transform.position = new Vector3(rowLength / 2 * blockWidth, UnityEngine.Camera.main.transform.position.y, columnLength / 2 * blockWidth);
    }
   
    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButtonDown(0)&&BaseClient.instance.isConnected)
        {
            //only for testing, needs to be improved once we know how we do the interaction (table/tablet..)
            Vector3 screenPos = Input.mousePosition;
            screenPos.z = UnityEngine.Camera.main.transform.position.y;
            Vector3 pos = UnityEngine.Camera.main.ScreenToWorldPoint(screenPos);
            var column = (int)((pos.x + blockWidth / 2) / blockWidth);
            var row = (int)((pos.z + blockWidth / 2) / blockWidth);
            var newBlockIdx = UnityEngine.Random.Range(0, mazeBlocksToGenerate.Length); //should be selected by the user in the future
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
        Net_MoveMazeMsg msg = new Net_MoveMazeMsg(idx, isRow, false, newBlockId);
        BaseClient.instance.SendToServer(msg);
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
        Net_MoveMazeMsg msg = new Net_MoveMazeMsg(idx, isRow, true,newBlockId);
        BaseClient.instance.SendToServer(msg);

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
        var newBlock = Instantiate(mazeBlocksToGenerate[newBlockId], pos, Quaternion.identity);
        var moveVector = inRow ? new Vector3((moveLeft ? -1 : 1) * blockWidth, 0, 0) : new Vector3(0, 0, (moveLeft ? -1 : 1) * blockWidth);
        var tween = newBlock.transform.DOMove(newBlock.transform.position + moveVector, moveTime);
        tween.OnComplete(() =>
        {
            if (inRow) mazeBlocks[moveLeft ? rowLength - 1 : 0, idx] = newBlock.transform;
            else mazeBlocks[idx, moveLeft ? columnLength - 1 : 0] = newBlock.transform;
        });
    }

}
