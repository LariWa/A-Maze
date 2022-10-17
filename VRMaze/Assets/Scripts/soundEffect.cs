using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundEffect : MonoBehaviour
{
    SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void playNormalF(){
        soundManager.Play("normalFootstep");
    }

    void playAttack(){
        soundManager.Play("attackFootstep");
    }
}
