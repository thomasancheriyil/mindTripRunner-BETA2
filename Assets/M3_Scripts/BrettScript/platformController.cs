using UnityEngine;
using System.Collections;

/* Using public variables level designer can change platform speed
 * Variables to be set direction, target velocity, force
 * 
 * Prefab defaults will be:
 * motor force = 500
 * motor speed = 15
 * CCW direction
 * rotate in z direction allowed
 * rotate off
 */
public class platformController : MonoBehaviour {

	public float force;				//force used to obtain target speed 
	public float speed; 			//target velocity of platform 15 is tested speed 
	public bool directionCW; 		//true CW; false CCW rotation 
	public bool enable;				//true platform moves; false '' stops 

	//CW motion sets arm 0 and 3 motors to +ve speed; arms 1 and 2 motors -ve

	public GameObject arm0;
	public GameObject arm1;
	public GameObject arm2;
	public GameObject arm3;
	public GameObject cube;

	HingeJoint joint0;
	HingeJoint joint1;
	HingeJoint joint2;
	HingeJoint joint3;
	Rigidbody platform;

	// Use this for initialization
	void Start () {

		joint0 = arm0.GetComponent<HingeJoint>();
		joint1 = arm1.GetComponent<HingeJoint> ();
		joint2 = arm2.GetComponent<HingeJoint>();
		joint3 = arm3.GetComponent<HingeJoint>();

		platform = cube.GetComponent<Rigidbody> ();
		platform.isKinematic = !enable;
	}

	//trigger is setup as box collider on game object and rendered as a separate game object (green box in example scene)
	//trigger position, size will need to be updated based on level design 
	/*triggered platform behavior:
	 * By default platform does not move
	 * When player enters trigger then rotation is enabled
	 * When player leaves platform continues to rotate
	 * When player re-enters the trigger then rotation is stopped
	 */
	void OnTriggerEnter(Collider other){

		if (other.gameObject.CompareTag ("Player"))
			enableRotate (!enable);
	}

	// Update is called once per frame
	void Update () {
	
		updateMotors (enable, directionCW,speed,force);
	}
		
	void updateMotors(bool e, bool d, float s, float f){

		if (e) {

			platform.isKinematic = !e;

			JointMotor m0 = joint0.motor;
			JointMotor m1 = joint1.motor;
			JointMotor m2 = joint3.motor;
			JointMotor m3 = joint3.motor;

			m0.force = f;
			m1.force = f;
			m2.force = f;
			m3.force = f;

			if (d) {
				//CW motion
				//set speed
				m0.targetVelocity = -s;
				m3.targetVelocity = -s;

				m1.targetVelocity = s;
				m2.targetVelocity = s;
			} else if (!d) {
				//CCW motion
				//set speed
				m0.targetVelocity = s;
				m3.targetVelocity = s;

				m1.targetVelocity = -s;
				m2.targetVelocity = -s;
			}

			joint0.motor = m0;
			joint1.motor = m1;
			joint2.motor = m2;
			joint3.motor = m3;

		} else if (!e) {
			
			//stop rotating the platform
			platform.isKinematic = !e;	

		}

	}

	public void enableRotate (bool rotate){
	
		enable = rotate;

	}
}
