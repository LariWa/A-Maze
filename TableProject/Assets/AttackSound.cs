using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSound : MonoBehaviour
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
    
    void AttackSoundFct()
    {
    	run.Play();
    }
}
