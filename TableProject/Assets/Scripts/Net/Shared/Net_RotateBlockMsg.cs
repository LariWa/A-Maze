using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_RotateBlockMsg : NetMessage
{
    // 0-8 OP CODE
    public Net_RotateBlockMsg()
    {
        Code = OpCode.ROTATEBLOCK_MSG;
    }
    public Net_RotateBlockMsg(DataStreamReader reader)
    {
        Code = OpCode.ROTATEBLOCK_MSG;
        Deserialize(reader);
    }
   
    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
    }
   
    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER: rotate");
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("Client: rotate");
    }
}
