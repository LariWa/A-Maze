using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sendRiddleFound : MonoBehaviour
{
    // Start is called before the first frame update
    private float lastSend;
    private Vector3 prevPos;
    private BaseServer server;
    public bool isPlayer;
    public int id; //each Enemy or object (objs placed by 2. player) should have a unique id!
    public float sendInterval = 0.2f;

    void Start()
    {
        server = FindObjectOfType<BaseServer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W))
        {
            Net_FoundRiddleMsg msg = new Net_FoundRiddleMsg();
            server.SendToClient(msg);
            lastSend = Time.time;
        }
    }

    bool timePassed()
    {
        return Time.time - lastSend > sendInterval;
    }
    bool posChanged()
    {
        return prevPos != transform.position;

    }
}
