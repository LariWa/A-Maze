using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class RiddleBlock : MonoBehaviour
{
    Animation animations;
    Material material;
    Vector3 directionToTarget;

    private BaseServer server;

    public float radius = 100000000;

    public GameObject playerRef;
    public SoundManager soundManager;
    public Transform player;
    
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public PathCreator pathCreator;
    public float distanceToTarget;

    // 0 : Not in the room 
    // 1 : Has entered the room 
    // 2 : Entered the room and doors have been closed
    // 3 : Completed the room challenge
    public int playerPosStatus = 0; 
    public static RiddleBlock instance { get; private set; }

    private void Start()
    {
        instance = this;
        server = FindObjectOfType<BaseServer>();
        soundManager = FindObjectOfType<SoundManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animations =GetComponent<Animation>();
    }


    public void openDoors(){
        animations.Play("openDoors");
        playerPosStatus = 3;
    }

    private void Update()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        //Get the position of the block center
        Vector3 centerPosition = transform.Find("ground_cross").position;
        float limitDist =  transform.Find("ground_cross").GetComponent<Renderer>().bounds.size[0];

        //Check the distance of the player from the block center
        Vector3 playerPos = player.position;
        distanceToTarget = Vector3.Distance(playerPos, centerPosition);

        //If the distance is inferior to the threshold, activate the doors
        if (distanceToTarget < limitDist/2){

            //Change playerpos status to has just entered the room
            if (playerPosStatus == 0) {
                playerPosStatus = 1;
            }

            if (playerPosStatus == 1) {
                animations.Play("closeDoors");
                playerPosStatus = 2; //Change status so that the animation only plays once
                Net_FoundRiddleMsg msg = new Net_FoundRiddleMsg();
                server.SendToClient(msg);
            }
        }
    }
    
}

