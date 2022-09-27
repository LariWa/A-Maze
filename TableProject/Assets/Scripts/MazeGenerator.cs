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
    public GameObject btnPrefab;
    public Transform mazeUI;
    Transform nextBlock;
    Vector3 nextBlockPos;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;

    }
    public void generateMaze(int columnLength, int rowLength, int blockWidth, int[] blockIdxs, int[] blockRotations)
    {
        this.columnLength = columnLength;
        this.rowLength = rowLength;
        this.blockWidth = blockWidth;
        mazeBlocks = new Transform[rowLength, columnLength];
        //place blocks on grid
        for (int i = 0; i < columnLength * rowLength; i++)
        {
            GameObject block = Instantiate(mazeBlocksToGenerate[blockIdxs[i]], new Vector3(i / columnLength * blockWidth, 0, i % columnLength * blockWidth), Quaternion.Euler(0, blockRotations[i] * 90, 0));
            mazeBlocks[i / columnLength, i % columnLength] = block.transform;
        }

        //place camera
        UnityEngine.Camera.main.transform.position = new Vector3(rowLength / 2 * blockWidth, UnityEngine.Camera.main.transform.position.y, columnLength / 2 * blockWidth);

        //place movement btns 
        //for rows
        for (int i = 1; i < columnLength - 1; i++)
        {
            //right
            createBtn(blockWidth, new Vector3(-blockWidth, 0, i * blockWidth), Quaternion.Euler(90, 0, -90), i, true, false);
            //left
            createBtn(blockWidth, new Vector3(rowLength * blockWidth, 0, i * blockWidth), Quaternion.Euler(90, 0, 90), i, true, true);
        }
        //for columns
        for (int i = 1; i < rowLength - 1; i++)
        {
            //up
            createBtn(blockWidth, new Vector3(i * blockWidth, 0, -blockWidth), Quaternion.Euler(90, 0, 0), i, false, false);
            //down
            createBtn(blockWidth, new Vector3(i * blockWidth, 0, columnLength * blockWidth), Quaternion.Euler(90, 0, 180), i, false, true);
        }


        //place next block
        nextBlockPos = new Vector3((rowLength + 0.5f) * blockWidth, 0, ((columnLength + 0.5f) * blockWidth));
        nextBlock = Instantiate(mazeBlocksToGenerate[1], nextBlockPos, Quaternion.identity).transform;
        nextBlock.localScale = Vector3.one * blockWidth;

    }
    void createBtn(int blockWidth, Vector3 pos, Quaternion rot, int index, bool isRow, bool moveNegDir)
    {
        var btn = Instantiate(btnPrefab, pos, rot, mazeUI);
        btn.transform.localScale = Vector3.one * blockWidth;
        btn.GetComponent<moveBtn>().Init(index, isRow, moveNegDir);
    }
    public void move(int index, bool isRow, bool moveNegDir)
    {
        if (nextBlock.position == nextBlockPos)
        {
            if (moveNegDir) moveInNegativeDir(isRow, index, 0);
            else moveInPositiveDir(isRow, index, 0);
            Net_MoveMazeMsg msg = new Net_MoveMazeMsg(index, isRow, moveNegDir);
            BaseClient.instance.SendToServer(msg);
        }
    }
    // Update is called once per frame
    void Update()
    {

        //if (Input.GetMouseButtonDown(0) && BaseClient.instance.isConnected)
        //{
        //    //only for testing, needs to be improved once we know how we do the interaction (table/tablet..)
        //    Vector3 screenPos = Input.mousePosition;
        //    screenPos.z = UnityEngine.Camera.main.transform.position.y;
        //    Vector3 pos = UnityEngine.Camera.main.ScreenToWorldPoint(screenPos);
        //    var column = (int)((pos.x + blockWidth / 2) / blockWidth);
        //    var row = (int)((pos.z + blockWidth / 2) / blockWidth);
        //    var newBlockIdx = UnityEngine.Random.Range(0, mazeBlocksToGenerate.Length); //should be selected by the user in the future
        //    if (pos.x + blockWidth / 2 < 0 && row < columnLength)//move row right
        //        moveInPositiveDir(true, row, newBlockIdx);
        //    else if ((pos.x + blockWidth / 2) / blockWidth > rowLength && row < columnLength) //move row left
        //        moveInNegativeDir(true, row, newBlockIdx);
        //    else if (pos.z + blockWidth / 2 < 0 && column < rowLength)//move column up
        //        moveInPositiveDir(false, column, newBlockIdx);
        //    else if (((pos.z + blockWidth / 2) / blockWidth > columnLength) && (column < rowLength))    //move column down
        //        moveInNegativeDir(false, column, newBlockIdx);
        //}
    }
    void moveInPositiveDir(bool isRow, int idx, int newBlockId)//right or up
    {
          for (int i = (isRow ? rowLength : columnLength) - 1; i >= 0; i--)
        {
            var block = isRow ? mazeBlocks[i, idx] : mazeBlocks[idx, i];
            var moveVector = isRow ? new Vector3(blockWidth, 0, 0) : new Vector3(0, 0, blockWidth);
            var tween = block.DOMove(block.position + moveVector, moveTime);
            if (isLastBlock(isRow, i, false))//delete last block
                tween.OnComplete(() =>
                {
                    nextBlock = block;
                    nextBlock.position = nextBlockPos;
                });
            else
            {
                if (isRow) mazeBlocks[i + 1, idx] = block;
                else mazeBlocks[idx, i + 1] = block;
            }
        }
        placeNewBlock(idx, isRow, false);

    }
    void moveInNegativeDir(bool isRow, int idx, int newBlockId)//left or down
    {
        for (int i = 0; i < (isRow ? rowLength : columnLength); i++)
        {
            var block = isRow ? mazeBlocks[i, idx] : mazeBlocks[idx, i];
            var moveVector = isRow ? new Vector3(-blockWidth, 0, 0) : new Vector3(0, 0, -blockWidth);
            var tween = block.DOMove(block.position + moveVector, moveTime);
            if (isLastBlock(isRow, i, true))//delete last block
                tween.OnComplete(() =>
                {
                    nextBlock = block;
                    nextBlock.position = nextBlockPos;
                });
            else
            {
                if (isRow) mazeBlocks[i - 1, idx] = block;
                else mazeBlocks[idx, i - 1] = block;
            }
        }
        placeNewBlock(idx, isRow, true);

    }
    bool isLastBlock(bool isRow, int idx, bool moveNegDirt)
    {
        if (isRow) return idx == rowLength - 1 && !moveNegDirt || idx == 0 && moveNegDirt;
        else return idx == columnLength - 1 && !moveNegDirt || idx == 0 && moveNegDirt;
    }

    void placeNewBlock(int idx, bool inRow, bool moveNegDirt)
    {
        Vector3 pos;
        if (inRow) pos = new Vector3(moveNegDirt ? rowLength * blockWidth : -blockWidth, 0, idx * blockWidth);
        else pos = new Vector3(idx * blockWidth, 0, moveNegDirt ? columnLength * blockWidth : -blockWidth);
        //var newBlock = Instantiate(mazeBlocksToGenerate[newBlockId], pos, Quaternion.identity);
        nextBlock.position = pos;
        var moveVector = inRow ? new Vector3((moveNegDirt ? -1 : 1) * blockWidth, 0, 0) : new Vector3(0, 0, (moveNegDirt ? -1 : 1) * blockWidth);
        var tween = nextBlock.transform.DOMove(nextBlock.transform.position + moveVector, moveTime);
        if (inRow) mazeBlocks[moveNegDirt ? rowLength - 1 : 0, idx] = nextBlock.transform;
        else mazeBlocks[idx, moveNegDirt ? columnLength - 1 : 0] = nextBlock.transform;
    }
}
