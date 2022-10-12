using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using UnityEngine.UI;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)] public float angle;

    public GameObject playerRef;
    public SoundManager soundManager;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer ;
    public int discoverPlayer = 0;
    public PathCreator pathCreator;
    Animation animations;
    public float distanceToTarget;

    Material material;
    Vector3 directionToTarget;
    public float speed = 5f;
    float distance = 0;
    public GameObject sword;
    public Slider lifeBar;

    private void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        // playerRef = GameObject.FindGameObjectWithTag("Player");
        //pathCreator = (PathCreator) GetComponent("PathCreator");
/*        StartCoroutine(FOVRoutine());
*/    }

/*    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }*/

    private void Update()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(-transform.forward, directionToTarget) < angle / 2)
            {
                print(directionToTarget);
                distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                {
                    canSeePlayer = true;
                    if (discoverPlayer == 0) {
                        discoverPlayer = 1;
                    }
                }
                else {
                    canSeePlayer = false;
                    discoverPlayer = 0;
                    soundManager.Stop("chase");
                }
            }
            else{
                canSeePlayer = false;
                discoverPlayer = 0;
                soundManager.Stop("chase");
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
            discoverPlayer = 0;
            soundManager.Stop("chase");
        }



        /*        material =GetComponent<Renderer>().material;
        */
        animations =GetComponent<Animation>();
        //Scream if it's the spider's first time seeing the player 
        if (discoverPlayer == 1)
        {
            soundManager.Play("scream");
            soundManager.Play("chase");
            discoverPlayer = 2;
        }

        animations.Play("Spider_Armature_run_ani_normal");
        if (canSeePlayer)
        {
            /*            material.color = Color.red;
            */
            //animations.Play("Spider_Armature_run_ani_attack");

            if (distanceToTarget > 0.2f){
                
                transform.position += new Vector3(directionToTarget.x/300, 0, directionToTarget.z/300) ;
            }

            float angle2 = Vector3.Angle(new Vector3(directionToTarget.x,0,directionToTarget.z), -transform.forward);
            if (angle2 > 2f)
            {
                transform.Rotate(0,angle2,0);

            }
        //    animations.Play("Attack");
        }
        else
        {
           distance -= speed * Time.deltaTime;
           transform.position = pathCreator.path.GetPointAtDistance(distance);
           transform.rotation = pathCreator.path.GetRotationAtDistance(distance) ;

            // transform.rotation = Quaternion.Euler(0,diff,0);
            // transform.Rotate(0,diff,0);

        }


        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == sword.name){
            lifeBar.value -= 0.3f;
            if (lifeBar.value <= 0)
            {
               // animations.Play("Spider_Armature|die");
                WaitForAnimation(animations);
                Destroy(this.gameObject);
            }
            BaseServer.instance.SendToClient(new Net_KillEnemyMsg(transform.position.x, transform.position.z));
        }
    }

    private IEnumerator WaitForAnimation(Animation anim)
    {
        do
        {
            yield return null;
        } while (anim.isPlaying);
    }
}