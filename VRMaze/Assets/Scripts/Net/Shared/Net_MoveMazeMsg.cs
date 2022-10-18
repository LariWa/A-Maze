using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_MoveMazeMsg : NetMessage
{
    // 0-8 OP CODE
    public int index;
    public bool moveLeft;
    public bool isRow;

    public Net_MoveMazeMsg()
    {
        Code = OpCode.MOVE_MAZE_MSG;
    }
    public Net_MoveMazeMsg(DataStreamReader reader)
    {
        Code = OpCode.MOVE_MAZE_MSG;
        Deserialize(reader);
    }
    public Net_MoveMazeMsg(int index, bool isRow, bool moveLeft)
    {
        Code = OpCode.MOVE_MAZE_MSG;
        this.index = index;
        this.moveLeft = moveLeft;
        this.isRow = isRow;

    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteInt((int)Code);
        writer.WriteInt(index);
        writer.WriteInt(moveLeft ? 1 : 0);
        writer.WriteInt(isRow ? 1 : 0);

    }

    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        index = reader.ReadInt();
        moveLeft = intToBool(reader.ReadInt());
        isRow = intToBool(reader.ReadInt());

    }
    bool intToBool(int value)
    {
        return (value == 1) ? true : false;
    }
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER: received move maze msg");
        MazeGenerator.instance.move(index, isRow, moveLeft);
    }
    public override void ReceivedOnClient()
    {
        // Debug.Log("Client:" + ChatMessage);
    }
}
