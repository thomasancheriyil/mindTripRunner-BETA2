using UnityEngine;
using System.Collections;

public class chaseStopTrigger : MonoBehaviour {
    public chase AI1Script, AI2Script;
    private BoxCollider boxCollider;
    private GameObject playerObject;                      // Reference to the player.
    public GameObject AI1;
    public GameObject AI2;
    // Use this for initialization
    void Start () {
        boxCollider = GetComponent<BoxCollider>();
        playerObject = GameObject.FindGameObjectWithTag("Player");
        //AI1 = GameObject.Find("Protect");
        //AI1 = GameObject.Find("Protect(1)");
        AI1Script = AI1.GetComponent<chase>();
        AI2Script = AI2.GetComponent<chase>();
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerStay(Collider other)
    {
        // If the player has entered the trigger sphere...
        if (other.gameObject == playerObject)
        {
            // By default the player is not in sight.
            Debug.Log("Player in sight");
            AI1Script.playerOutofSight = true;
            AI2Script.playerOutofSight = true;


        }
    }
    void OnTriggerExit(Collider other)
    {
        // If the player leaves the trigger zone...
        if (other.gameObject == playerObject)
        {
            // ... the player left the box.
            AI1Script.playerOutofSight = false;
            AI2Script.playerOutofSight = false;
        }
    }
}
