using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_ObjInteraction_MSg : NetMessage
{
    pickUpObjCode objCode;
    bool useObj;

    public Net_ObjInteraction_MSg(pickUpObjCode code, bool use)
    {
        Code = OpCode.OBJ_INNTERACTION_MSG;
        objCode = code;
        useObj = use;
        
        }
    public Net_ObjInteraction_MSg(DataStreamReader reader)
    {
        Code = OpCode.OBJ_INNTERACTION_MSG;
        objCode =(pickUpObjCode) reader.ReadByte();
        useObj = intToBool(reader.ReadInt());

        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteInt((int)Code);
        writer.WriteByte((byte)objCode);

        writer.WriteInt(useObj ? 1 : 0);
    }

    public override void ReceivedOnServer()
    {
    }
    public override void ReceivedOnClient()
    {
        Inventory.instance.OnMsg(objCode, useObj);
    }
    bool intToBool(int value)
    {
        return (value == 1) ? true : false;
    }
}
