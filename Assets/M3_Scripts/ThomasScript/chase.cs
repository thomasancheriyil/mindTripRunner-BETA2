using UnityEngine;
using System.Collections;

public class chase : MonoBehaviour {
    public Transform player;
    public Transform head;
    Animator anim;
    bool pursuing = false;
    public float viewAngle = 30f;
    public double stoppingDistance = 1.5f;
    public float viewDistance = 10f;
    private NavMeshAgent nav;                               // Reference to the nav mesh agent.
    private GameObject playerObject;                      // Reference to the player.
    private float chaseTimer = 0f;
    public float chaseWaitTime = 20f;
    private SphereCollider sphereCollider;                  //Sphere collider for the AI
    public bool playerInSight;                      // Whether or not the player is currently sighted.
    public bool playerOutofSight;                   // Whether player has left the NavMesh
    public GameObject homePoint;
    public AudioSource chaseAudio;
    public AudioSource swordAudio;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        this.tag = "Untagged";
        sphereCollider = GetComponent<SphereCollider>();
        playerOutofSight = false;
        //audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update () {
        Vector3 direction = player.position - this.transform.position;
        direction.y = 0;
        float angle = Vector3.Angle(direction, head.up);
        RaycastHit hit, hit2;
        
        if (playerOutofSight == false && Vector3.Distance(player.position, this.transform.position) < viewDistance && (angle < viewAngle || pursuing) && chaseTimer < chaseWaitTime && playerInSight)
        {
            // ... and if a raycast towards the player hits something...
            bool phy1 = Physics.Raycast(transform.position + transform.up, direction.normalized, out hit, viewDistance);
            bool phy2 = Physics.Raycast(transform.position + (transform.up / 2), direction.normalized, out hit2, viewDistance);
            // ... and if the raycast hits the player...
            if ((phy1 && hit.collider.gameObject == playerObject) || (phy2 && hit2.collider.gameObject == playerObject))
            {
                pursuing = true;

                this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                                            Quaternion.LookRotation(direction), 0.1f);

                anim.SetBool("isIdle", false);
                if (direction.magnitude > stoppingDistance)
                {
                    nav.Resume();
                    nav.SetDestination(player.position);
                    //this.transform.Translate(0, 0, 0.05f);
                    anim.SetBool("isWalking", true);
                    anim.SetBool("isAttacking", false);
                    if(chaseAudio.isPlaying == false)
                        chaseAudio.Play();
                    if(swordAudio.isPlaying)
                        swordAudio.Stop();
                }
                else
                {
                    Debug.Log("Attack!");
                    if (swordAudio.isPlaying == false)
                        swordAudio.Play();
                    this.tag = "Obstacle";
                    nav.Stop();
                    anim.SetBool("isAttacking", true);
                    anim.SetBool("isWalking", false);
                    chaseTimer = 0f;
                }
            }
            else
            {
                if (swordAudio.isPlaying == true)
                    swordAudio.Stop();
                chaseTimer += Time.deltaTime;
                nav.SetDestination(player.position);
                    
            }



        }
        else
        {
            chaseTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if (chaseTimer >= chaseWaitTime)
            {
                if(chaseAudio.isPlaying)
                    chaseAudio.Stop();
                nav.SetDestination(homePoint.transform.position);
                float dist =nav.remainingDistance;
                if (dist != Mathf.Infinity && nav.pathStatus == NavMeshPathStatus.PathComplete && nav.remainingDistance == 0)
                {
                    nav.Stop();
                    anim.SetBool("isIdle", true);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isAttacking", false);
                }
                        
                pursuing = false;
                chaseTimer = 0f;
            }
        }
        
        //this.tag = "Untagged";
    }

    void OnTriggerStay(Collider other)
    {
        // If the player has entered the trigger sphere...
        if (other.gameObject == playerObject)
        {
            // By default the player is not in sight.
            playerInSight = true;

            
           
        }
    }


    void OnTriggerExit(Collider other)
    {
        // If the player leaves the trigger zone...
        if (other.gameObject == playerObject)
            // ... the player is not in sight.
            playerInSight = false;
    }


}
