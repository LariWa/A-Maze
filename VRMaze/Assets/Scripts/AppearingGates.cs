using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearingGates : MonoBehaviour
{
    public AudioClip sound;
    GameObject[] gates ;

    // Start is called before the first frame update
    void Start()
    {
      gates = GameObject.FindGameObjectsWithTag("Gate");
      foreach (GameObject gate in gates)
      {
        gate.GetComponent<Renderer>().enabled = false;
      }

      GetComponent<AudioSource>().playOnAwake = false;
      GetComponent<AudioSource>().clip = sound;
    }

    void OnTriggerEnter() {
      foreach (GameObject gate in gates)
      {
        gate.GetComponent<Renderer>().enabled = true;
      }
      
      GetComponent<AudioSource>().Play();
      Destroy(this);
    }

}
