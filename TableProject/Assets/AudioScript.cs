using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour
{
    public AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void PlayNormal(){
    	audioManager.Play("normalFootsteps");
    }
    
    void PlayAttack(){
    	audioManager.Play("chaseFootsteps");
    }
}
