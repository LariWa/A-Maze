using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keepAlive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("sendMsg", 1.0f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void sendMsg()
    {
        if (BaseClient.instance.isConnected)
        {
            BaseClient.instance.SendToServer(new Net_ChatMessage("keep alive"));
        }
    }
}
