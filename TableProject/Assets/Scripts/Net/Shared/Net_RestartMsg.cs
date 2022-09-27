using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_RestartMsg : NetMessage
{
    // 0-8 OP CODE
    public Net_RestartMsg()
    {
        Code = OpCode.RESTART_MSG;
    }
    public Net_RestartMsg(DataStreamReader reader)
    {
        Code = OpCode.CHAT_MESSAGE;
        Deserialize(reader);
    }
   
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
    }
   
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER: restart");
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("Client: restart");
    }
}
