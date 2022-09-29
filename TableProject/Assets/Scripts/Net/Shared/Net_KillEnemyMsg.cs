using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Networking.Transport;
using UnityEngine;

public class Net_KillEnemyMsg : NetMessage
{
    public float posX { set; get; }
    public float posZ { set; get; }

    public Net_KillEnemyMsg(float x,  float z)
    {
        Code = OpCode.KILLENEMY_MSG;
            posX = x;
            posZ = z;
        }
    public Net_KillEnemyMsg(DataStreamReader reader)
    {
        Code = OpCode.KILLENEMY_MSG;
        posX = reader.ReadFloat();
        posZ = reader.ReadFloat();
        Deserialize(reader);
    }

    public override void Serialize(ref DataStreamWriter writer)
    {
        writer.WriteByte((byte)Code);
        writer.WriteFloat(posX);
        writer.WriteFloat(posZ);
    }

    public override void ReceivedOnServer()
    {
        Debug.Log("SERVER: restart");
        MazeGenerator.instance.killEnemy(posX, posZ);
    }
    public override void ReceivedOnClient()
    {
        Debug.Log("Client: restart");
    }
}
