using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_MazeGenerationMsg : NetMessage
{
    // 0-8 OP CODE
    public int columnLength { set; get; }
    public int rowLength { set; get; }
    public int blockWidth { set; get; }

    public int[] arrayOfBlockIdx { set; get; }
    public int[] blockRotations { set; get; }


    public Net_MazeGenerationMsg()
    {
        Code = OpCode.MAZE_GENERATION_MSG;
    }
    public Net_MazeGenerationMsg(DataStreamReader reader)
    {
        Code = OpCode.MAZE_GENERATION_MSG;
        Deserialize(reader);
    }
    public Net_MazeGenerationMsg(int columnLength, int rowLength, int blockWidth, int[] arrayOfBlockIdx, int[] blockRotations)
    {
        Code = OpCode.MAZE_GENERATION_MSG;
        this.columnLength = columnLength;
        this.rowLength = rowLength;
        this.blockWidth = blockWidth;
        this.arrayOfBlockIdx = arrayOfBlockIdx;
        this.blockRotations = blockRotations;

    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteInt(columnLength);
        writer.WriteInt(rowLength);
        writer.WriteInt(blockWidth);
        writer.WriteFixedString512(string.Join(",", arrayOfBlockIdx));
        writer.WriteFixedString512(string.Join(",", blockRotations));


    }
    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        columnLength = reader.ReadInt();
        rowLength = reader.ReadInt();
        blockWidth = reader.ReadInt();
        arrayOfBlockIdx = Array.ConvertAll(("" + reader.ReadFixedString512()).Split(','), int.Parse);
        blockRotations = Array.ConvertAll(("" + reader.ReadFixedString512()).Split(','), int.Parse);
    }
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER: MazeGeneration received");
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("received maze generation msg");


    }
}
