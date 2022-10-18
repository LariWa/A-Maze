using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int columnLength, rowLength;
    public int blockWidth;
    public float blockHeight;
    [Serializable]
    public struct MazeBlockPrefab
    {
        public int quantity;
        public GameObject prefab;
    }
    public MazeBlockPrefab[] mazeBlocksToGenerate;
    Transform[,] mazeBlocks;
    public float moveTime = 5.0f;
    Net_MazeGenerationMsg msg;
    Transform nextBlock;
    public GameObject playerBlock, finishBlock, cornerBlock;
    public static MazeGenerator instance { get; private set; }
    public GameObject wall;
    public Transform player;
    Transform initPosPlayer;

    public SoundManager soundManager;


    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        generateMaze();
        initPosPlayer = player.transform;
        soundManager = FindObjectOfType<SoundManager>();
    }
    void generateMaze()
    {
        mazeBlocks = new Transform[rowLength, columnLength];
        //create array with all the blocks we want to use
        var allMazeBlocks = new GameObject[columnLength * rowLength - 2];
        int idxAllMazeBlocks = 0;
        int idxBlockDictionary = 0;
        foreach (MazeBlockPrefab mazeBlock in mazeBlocksToGenerate)
        {
            var blockScript = mazeBlock.prefab.GetComponent<MazeBlock>();
            if (blockScript == null)
                blockScript = mazeBlock.prefab.AddComponent<MazeBlock>();
            blockScript.id = idxBlockDictionary;
            idxBlockDictionary++;
            for (int i = 0; i < mazeBlock.quantity; i++)
            {
                if (idxAllMazeBlocks < columnLength * rowLength - 2)
                {
                    allMazeBlocks[idxAllMazeBlocks] = mazeBlock.prefab;
                    idxAllMazeBlocks++;
                }
                else
                    Debug.LogWarning("number of maze blocks is bigger than cell amount, please fix!");
            }
        }
        if (idxAllMazeBlocks < columnLength * rowLength - 2)
            Debug.LogWarning("number of maze blocks is smaller than cell amount, please fix!");


        randomizeArray(allMazeBlocks);


        var blockRotations = new int[rowLength * columnLength - 2];

        //place blocks on grid
        mazeBlocks[0, 0] = Instantiate(playerBlock, Vector3.zero, playerBlock.transform.rotation, transform).transform;
        mazeBlocks[rowLength - 1, columnLength - 1] = Instantiate(finishBlock, new Vector3((rowLength - 1) * blockWidth, 0, (columnLength - 1) * blockWidth), finishBlock.transform.rotation, transform).transform;

        for (int i = 0; i < columnLength * rowLength - 2; i++)
        {
            int rot = UnityEngine.Random.Range(0, 3);
            blockRotations[i] = rot;
            Vector3 pos = new Vector3((i + 1) / columnLength * blockWidth, 0, (i + 1) % columnLength * blockWidth);
            if ((i + 1) / columnLength == rowLength - 1 && (i + 1) % columnLength == 0)
            { //left upper corner block
                Debug.Log("cornerblock");
                Instantiate(cornerBlock, pos, Quaternion.Euler(0, 90, 0), transform);


            }
            else if ((i + 1) % columnLength == columnLength - 1 && (i + 1) / columnLength == 0)
            {
                 Instantiate(cornerBlock, pos, Quaternion.Euler(0, -90, 0), transform);
            }

            else
            {
                GameObject block = Instantiate(allMazeBlocks[i], pos, Quaternion.Euler(0, 0, 0), transform);
                if (block.transform.Find("Spider1"))
                {
                    block.transform.Find("Spider1").GetComponent<FieldOfView>().id = i+1;
                }
                mazeBlocks[(i + 1) / columnLength, (i + 1) % columnLength] = block.transform; //0 is player block
            }
        }
        //save msg for client
        var netMsgBlockArray = allMazeBlocks.ToList().ConvertAll<int>(elem => elem.GetComponent<MazeBlock>().id).ToArray();
        msg = new Net_MazeGenerationMsg(columnLength, rowLength, blockWidth, netMsgBlockArray, blockRotations);

        nextBlock = Instantiate(mazeBlocksToGenerate[0].prefab, Vector3.one, Quaternion.identity, transform).transform;
        //nextBlock.localScale = Vector3.one * blockWidth;
        nextBlock.gameObject.SetActive(false);

        //save player
        //  player = mazeBlocks[0, 0].transform.Find("Player");
        //wallLeft
        var wallLeft = Instantiate(wall, transform);
        wallLeft.transform.localScale = new Vector3(0.01f, blockHeight, columnLength * blockWidth);
        wallLeft.transform.position = new Vector3(-blockWidth / 2f, blockHeight / 2f, ((float)(columnLength * blockWidth) / 2) - blockWidth / 2);

        var wallRight = Instantiate(wall, transform);
        wallRight.transform.localScale = new Vector3(0.01f, blockHeight, columnLength * blockWidth);
        wallRight.transform.position = new Vector3(rowLength * blockWidth - blockWidth / 2f, blockHeight / 2f, ((float)(columnLength * blockWidth) / 2) - blockWidth / 2);

        var wallDown = Instantiate(wall, transform);
        wallDown.transform.localScale = new Vector3(rowLength * blockWidth, blockHeight, 0.01f);
        wallDown.transform.position = new Vector3(((float)(rowLength * blockWidth) / 2) - blockWidth / 2, blockHeight / 2, -blockWidth / 2);
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        wallDown.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 180);

        var wallUp = Instantiate(wall, transform);
        wallUp.transform.localScale = new Vector3(rowLength * blockWidth, blockHeight, 0.01f);
        wallUp.transform.position = new Vector3(((float)(rowLength * blockWidth) / 2) - blockWidth / 2, blockHeight / 2, columnLength * blockWidth - blockWidth / 2f);
    }
    void randomizeArray(GameObject[] array)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int rnd = UnityEngine.Random.Range(i, array.Length);
            var temp = array[rnd];
            array[rnd] = array[i];
            array[i] = temp;
        }
    }
    public void sendToClient()
    {
        Debug.Log(msg);
        BaseServer.instance.SendToClient(msg);

    }
    // Update is called once per frame

    public void move(int idx, bool isRow, bool moveLeft)
    {
        //movePlayerWithMaze();
        if (moveLeft)
            moveInNegativeDir(isRow, idx);
        else
            moveInPositiveDir(isRow, idx);

        soundManager.Play("slideDoors");

    }
    void moveInPositiveDir(bool isRow, int idx)//right or up
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
                    nextBlock.gameObject.SetActive(false);
                });
            else
            {
                if (isRow) mazeBlocks[i + 1, idx] = block;
                else mazeBlocks[idx, i + 1] = block;
            }
        }
        placeNewBlock(idx, isRow, false);

    }
    void moveInNegativeDir(bool isRow, int idx)//left or down
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
                    nextBlock.gameObject.SetActive(false);
                });
            else
            {
                if (isRow) mazeBlocks[i - 1, idx] = block;
                else mazeBlocks[idx, i - 1] = block;
            }
        }
        placeNewBlock(idx, isRow, true);

    }
    bool isLastBlock(bool isRow, int idx, bool moveLeft)
    {
        if (isRow) return idx == rowLength - 1 && !moveLeft || idx == 0 && moveLeft;
        else return idx == columnLength - 1 && !moveLeft || idx == 0 && moveLeft;
    }

    void placeNewBlock(int idx, bool inRow, bool moveLeft)
    {
        Vector3 pos;
        if (inRow) pos = new Vector3(moveLeft ? rowLength * blockWidth : -blockWidth, 0, idx * blockWidth);
        else pos = new Vector3(idx * blockWidth, 0, moveLeft ? columnLength * blockWidth : -blockWidth);
        nextBlock.position = pos;
        nextBlock.gameObject.SetActive(true);

        var moveVector = inRow ? new Vector3((moveLeft ? -1 : 1) * blockWidth, 0, 0) : new Vector3(0, 0, (moveLeft ? -1 : 1) * blockWidth);
        var tween = nextBlock.transform.DOMove(nextBlock.transform.position + moveVector, moveTime);

        if (inRow) mazeBlocks[moveLeft ? rowLength - 1 : 0, idx] = nextBlock.transform;
        else mazeBlocks[idx, moveLeft ? columnLength - 1 : 0] = nextBlock.transform;

    }
    public void restart()
    {
        Debug.Log("restart");
        for (var i = 0; i < this.transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
        generateMaze();
        sendToClient();
        Debug.Log(initPosPlayer.transform.position);
        player.position = Vector3.zero;
        player.rotation = Quaternion.identity;
        Debug.Log(initPosPlayer.transform.position);


    }
    void movePlayerWithMaze()
    {
        //  var column = (int)((player.position.x + blockWidth / 2) / blockWidth);
        //  var row = (int)((player.position.z + blockWidth / 2) / blockWidth);
        //  Debug.Log(column + " " + row);
        //player.parent = mazeBlocks[column, row];
    }
    public void rotateBlock()
    {
        nextBlock.Rotate(0, 90, 0);
    }

}
