using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class DangerRoom : MonoBehaviour
{
    Animation animations;
    Material material;
    Vector3 directionToTarget;

    public float radius = 100000000;

    public GameObject playerRef;
    public SoundManager soundManager;
    public Transform player;
    public List<GameObject> spiderList;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public PathCreator pathCreator;
    public float distanceToTarget;

    // 0 : Not in the room 
    // 1 : Has entered the room 
    // 2 : Entered the room and doors have been closed
    private int playerPosStatus = 0; 

    public void doorSound() {
        soundManager.Play("cellDoor");
    }
    //Check if there is any live spider left
    private bool anySpiderRemaining() 
    {
        foreach(GameObject spider in spiderList){
            if (spider != null) {
                return true;
            }
        }
        return false;
    }

    //Activate all spiders
    private void spawnSpiders()
    {
        foreach(GameObject spider in spiderList){
            spider.SetActive(true);
        }
    }

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animations =GetComponent<Animation>();

        //Initiate the spiders list :  
        //TODO : Mon dieu c'est dégeulasse, automatise had lkhra
        int i = 1;
        Transform currentSpider = transform.Find("Spider"+i.ToString());

        while (currentSpider != null) {
            currentSpider.gameObject.SetActive(false);
            spiderList.Add(currentSpider.gameObject);

            i++;
            currentSpider = transform.Find("Spider"+i.ToString());
        }
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
                this.spawnSpiders();
            }
        }

        //Open Doors when all spiders are killed
        if (!anySpiderRemaining() && playerPosStatus != 3) {
            playerPosStatus = 3;
            animations.Play("openDoors");
        }
    }
    
}

