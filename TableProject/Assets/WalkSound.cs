using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource run;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void WalkSoundFct()
    {
    	run.Play();
    }
}
