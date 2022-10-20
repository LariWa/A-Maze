using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;


public class scroll : MonoBehaviour
{
    public RiddleBlock riddleRoom;
    
    private MeshRenderer mesh;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {   
        if (RiddleBlock.instance.playerPosStatus == 2) {
            this.gameObject.SetActive(true);
        } else {
            this.gameObject.SetActive(false);
        }
        
    }
}
