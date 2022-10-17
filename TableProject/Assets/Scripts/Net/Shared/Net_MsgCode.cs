using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_MsgCode : NetMessage
{
    // 0-8 OP CODE
    public actionTypeCode actionType;

    public Net_MsgCode(actionTypeCode actionType)
    {
        Code = OpCode.CODE_MSG;
        this.actionType = actionType;
    }
    public Net_MsgCode(DataStreamReader reader)
    {
        Code = OpCode.CODE_MSG;
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteByte((byte)actionType);
    }
    public override void Deserialize(DataStreamReader reader)
    {
        //the first byte is handled already 
        actionType = (actionTypeCode)reader.ReadByte();
    }

    public override void ReceivedOnServer()
    {
   
    }
    public override void ReceivedOnClient()
    {
    }
}
