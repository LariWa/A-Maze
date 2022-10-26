using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_FoundRiddleMsg : NetMessage
{
    // 0-8 OP CODE
    // 8-256 String Message
    public FixedString128Bytes RiddleMessage { set; get; }
    public Net_FoundRiddleMsg()
    {
        Code = OpCode.FOUND_RIDDLE_MSG;
    }
    public Net_FoundRiddleMsg(DataStreamReader reader)
    {
        Code = OpCode.FOUND_RIDDLE_MSG;
        Deserialize(reader);
    }
    public Net_FoundRiddleMsg(string msg)
    {
        Code = OpCode.FOUND_RIDDLE_MSG;
        RiddleMessage = msg;
    }
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteInt((int)Code);
        writer.WriteFixedString128(RiddleMessage);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        RiddleMessage = reader.ReadFixedString128();
    }
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER:" + RiddleMessage);
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("Client:" + RiddleMessage);
        WindowUIManager.instance.showRiddle();
    }
}
