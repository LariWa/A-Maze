using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_KillEnemyMsg : NetMessage
{
    public float posX { set; get; }
    public float posZ { set; get; }
    public int index;
    public Net_KillEnemyMsg(float x, float z, int idx)
    {
        Code = OpCode.KILLENEMY_MSG;
        posX = x;
        posZ = z;
        index = idx;
    }
    public Net_KillEnemyMsg(DataStreamReader reader)
    {
        Code = OpCode.KILLENEMY_MSG;
        posX = reader.ReadFloat();
        posZ = reader.ReadFloat();
        index = reader.ReadInt();
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteInt((int)Code);
        writer.WriteFloat(posX);
        writer.WriteFloat(posZ);
        writer.WriteInt(index);
    }

    public override void ReceivedOnServer()
    {
    }
    public override void ReceivedOnClient()
    {

    }
}
