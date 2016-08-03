using UnityEngine;
using System.Collections;

public class ballSpawn : MonoBehaviour {
	// C#

	// Require the rocket to be a rigidbody.
	// This way we the user can not assign a prefab without rigidbody
	rotateObj clone;
	float last_shoot;
	private Rigidbody cannonBall;
	private initialCannon param;

	void Start () {
		param = GameObject.Find ("cannon").GetComponent<initialCannon>();
		clone = GameObject.Find("cannonBarrel").GetComponent<rotateObj>();
		last_shoot = 0.0F;
		cannonBall = param.cannonBall;
	}
	
	// Update is called once per frame
	void Update () {
		bool shootornot = clone.pur;
		//if (Input.GetKeyDown("l")){ // This must be viewing cone
		if (shootornot && Time.time - last_shoot > param.fireTimeGap){
			//transform.position and Quanterion must be updated
			Rigidbody A = (Rigidbody) Instantiate(cannonBall, transform.position, Quaternion.identity);
			//Instantiate(cannonBall, transform.position, Quaternion.identity);
			//Rigidbody A_rigid = A.GetComponent<Rigidbody> ();
			// Velocity of regidbody must be predicted
			Vector3 elevation = transform.forward;
			A.velocity = elevation * param.fireSpeed;
			//Ins
			// play sound when hit the object
			// disappear somehow
			last_shoot = Time.time;
			Destroy (A.gameObject, 5.0F);
		}
	}
	}
