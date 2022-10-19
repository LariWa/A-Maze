using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_RiddleAnswerMsg : NetMessage
{
    // 0-8 OP CODE
    // 8-256 String Message
    public FixedString128Bytes RiddleMessage { set; get; }
    public int isCorrectAnswer;

    public Net_RiddleAnswerMsg()
    {
        Code = OpCode.RIDDLE_ANSWER_MSG;
    }
    public Net_RiddleAnswerMsg(bool answer)
    {
        Code = OpCode.RIDDLE_ANSWER_MSG;
        isCorrectAnswer = answer == true ? 1 : 0;
    }
    public Net_RiddleAnswerMsg(DataStreamReader reader)
    {
        Code = OpCode.RIDDLE_ANSWER_MSG;
        Deserialize(reader);
    }
    public Net_RiddleAnswerMsg(string msg)
    {
        Code = OpCode.RIDDLE_ANSWER_MSG;
        RiddleMessage = msg;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteFixedString128(RiddleMessage);
        writer.WriteInt(isCorrectAnswer);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        RiddleMessage = reader.ReadFixedString128();
        isCorrectAnswer = reader.ReadInt();
    }
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER:" + RiddleMessage);
        Debug.Log("Answer : "+ this.isCorrectAnswer);
        RiddleBlock.instance.openDoors();
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("Client:" + RiddleMessage);
        Debug.Log("Answer : "+ this.isCorrectAnswer);
        RiddleBlock.instance.openDoors();
    }
}
