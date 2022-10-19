using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;



public class Mouvement : MonoBehaviour
{
    public float walkspeed;
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;
    public GameObject[] spiders;
    Animation animations;
    bool breaking = false;
    bool die = false;
    public int spidersNb;
    public GameObject cam;
    public Image HealthBar;
    ParticleSystem greenLight;
    public Vector3 lastForward;

    // Start is called before the first frame update
    void Start()
    {
        // animations = GetComponent<Animation>();
        LayerMask spiderMask =  LayerMask.GetMask("Spider");
        spidersNb = Physics.OverlapSphere(transform.position, 0.3f, spiderMask).Length;
        greenLight = (ParticleSystem) GameObject.FindGameObjectWithTag("GreenLight").GetComponent("ParticleSystem");
        greenLight.Stop();

    }

    // Update is called once per frame
    void Update()
    {


        //transform.rotation = Quaternion.Euler(0, transform.rotation.y, 0);
        LayerMask spiderMask =  LayerMask.GetMask("Spider");
        spidersNb = Physics.OverlapSphere(transform.position, 0.3f, spiderMask).Length;


        float speed = walkspeed * (1f + 2f * OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger)) ;
        string animate;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = 3 * walkspeed;

        }


       /* if (OVRInput.Get(OVRInput.Button.One))
        {
            // transform.Translate(0, 0, speed * Time.deltaTime);
            transform.position += speed * Time.deltaTime * cam.transform.forward.normalized;
        }*/

        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickUp))
        {
            
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
            {
                //transform.Translate(0, 0, speed * Time.deltaTime);
                transform.position += speed * Time.deltaTime * lastForward;
            }
            else
            {
                transform.position += speed * Time.deltaTime * cam.transform.forward.normalized;
                lastForward = cam.transform.forward.normalized;
            }
        }
        if (OVRInput.Get(OVRInput.Button.SecondaryThumbstickDown))
        {
            if (OVRInput.Get(OVRInput.RawButton.LIndexTrigger))
            {
                transform.Translate(0, 0, - speed * Time.deltaTime);
                transform.position -= speed * Time.deltaTime * lastForward;

            }
            else
            {
                transform.position -= speed * Time.deltaTime * cam.transform.forward.normalized;
                lastForward = cam.transform.forward.normalized;
            }
        }

        if (Input.GetKey(inputFront))
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
            // animations.Play(animate);
        }

        if (Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, -speed* Time.deltaTime);
            }
        if (Input.GetKey(inputLeft))
        {
            transform.Rotate(0, -0.5f, 0);
        }
        if (Input.GetKey(inputRight))
        {
            transform.Rotate(0, 0.5f, 0);
        }


        Collider[] potions = Physics.OverlapSphere(transform.position, 0.1f, LayerMask.GetMask("Potion"));
        if (potions.Length > 0)
        {
            HealthBar.fillAmount += 0.3f;
            Destroy(potions[0].gameObject);
            greenLight.Play();
        }
    }

 /*   private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Potion")
        {
            HealthBar.fillAmount += 0.3f;
            Destroy(other.gameObject);
        }
    }*/


}
