using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFlicker : MonoBehaviour
{

    public float minIntensity = 0f;
    public float maxIntensity = 10f;

    GameObject[] lamps ;
    Coroutine flickerAnimation;
    bool running = false;

    // Start is called before the first frame update
    void Start()
    {
      lamps = GameObject.FindGameObjectsWithTag("Lamp");
    }

    // Update is called once per frame
    void Update()
    {
      if (running == false) {
        flickerAnimation = StartCoroutine(flicker());
      }
    }

    IEnumerator flicker()
    {
      running = true;
      Transform spot = this.transform.Find("spot");
      Transform emitter = this.transform.Find("emitter");

      GameObject light = spot.gameObject;
      GameObject top = emitter.gameObject;

      float random = Random.Range(10f, 100f);
      yield return new WaitForSeconds(random);

      light.GetComponentInChildren<Light>().intensity = 0;
      top.GetComponent<Renderer>().enabled = false;
      random = Random.Range(0f, 10f);
      yield return new WaitForSeconds(random);

      light.GetComponentInChildren<Light>().intensity = 10;
      top.GetComponent<Renderer>().enabled = true;
      yield return new WaitForSeconds(0.2f);

      light.GetComponentInChildren<Light>().intensity = 0;
      top.GetComponent<Renderer>().enabled = false;
      yield return new WaitForSeconds(0.3f);

      light.GetComponentInChildren<Light>().intensity = 10;
      top.GetComponent<Renderer>().enabled = true;
      yield return new WaitForSeconds(0.2f);

      light.GetComponentInChildren<Light>().intensity = 0;
      top.GetComponent<Renderer>().enabled = false;
      yield return new WaitForSeconds(0.5f);

      light.GetComponentInChildren<Light>().intensity = 10;
      top.GetComponent<Renderer>().enabled = true;
      running = false;
      StopCoroutine(flickerAnimation);

    }
}
