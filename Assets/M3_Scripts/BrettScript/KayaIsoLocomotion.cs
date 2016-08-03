using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/*Controller information for input settings
	 * 
	 * Keyboard: WASD movement
	 * W/S - vertical axis
	 * A/D - horizontal axis
	 * C - crouch toggle
	 * space - jump
	 * [ - rotata camera left
	 * ] - rotate camera right
	 * 
	 * key map for xbox 360 (Windows/macOS/Linux) http://wiki.unity3d.com/index.php?title=Xbox360Controller
	 * XBOX360 (Windows ONLY): Left joystick movement 
	 * Left stick - vertical/horizontal axis
	 * Left stick click - crouch toggle
	 * A - jump 
	 * left bumper - rotate camera left
	 * right bumper - rotate camera right
	 * 
	 * key map for ps3 (macOS only) http://www.grapefruitgames.com/blog/2011/05/ps3-controllers-osx-unity-3d-together-at-last/
	 * PS3 (MacOS ONLY): Left joystick movement
	 * Left stick - vertical/horizontal axis
	 * L3 - crouch toggle
	 * X - jump
	 * L1 - rotate camera left
	 * R1 - rotate camera right
	 * 
	 */

public class KayaIsoLocomotion : MonoBehaviour
{
    
	private Vector3 mainCamForward;
	private Vector3 mainCamRight;
	private Vector3 moveInput;

	//variables for mecanim AC
	private Animator anim;
	private float turn;
	private float forward;
	private float animSpeed;
	private float animDirection;
	public float timeToRagdoll;
	private AnimatorStateInfo currentBaseState;
	public float jumpPower;
	//translates transform by x*jumpPower/2, jumpPower, z*jumpPower/2 in addition to root motion
	//public float jumpSmoothing;
	private float capsuleHeight;
	private Vector3 capsuleCenter;

	static int moveAnimState = Animator.StringToHash ("Base Layer.WalkRun");
	static int jumpAnimState = Animator.StringToHash ("Base Layer.Jumping");

	//info about ground
	public float groundHeight;
	//compensation in y-direction when checking for ground
	public float maxFallDistance;
	//max distance to ground; player rag dolls at large heights
	public LayerMask mask;
	//layers for capsule check, raycast, etc. to ignore
	static float origDistToGround;
	//distance from player (collider) center to the ground
	private float distToGround;
	//current distance to ground; defined as origDistToGround on Start()
	private Vector3 groundNormal;
	//current normal of walking plane
	private Vector3 landingPoint;
	//point where player will land if not grounded
    
	struct groundInfo
	{
		public bool onGround;
		public float distance;
		public Vector3 landing;
		public Vector3 normal;

	};

	private IsometricCamera isoCam;
	private Transform cam;
	private int camIndex;	//current view of the camera

	private Rigidbody kayaRB;
	private CapsuleCollider kayaCollider;
	private PhysicMaterial startingMaterial;
	public PhysicMaterial platformMaterial;	//high friction material to improve player interaction with moving platforms

	public Component[] avatarBones; //array of bone rigid bodies in player game object
    
	public bool ragDoll = false;
	//condition to enable ragdoll simulation when colliding with game object tagged as obstacle
	private bool jump = false;
	//condition indicating user has requested jump
	private bool crouch = false;
	//condition indication user has requested crouch
	private bool isGrounded = true;
	//transform for blood particle system

	//controller polish efx
	public Transform bloodParticle;
	//transform for jump dust system
	public Transform footParticle;
	//ragdoll sound
	private AudioSource wilhelmDeath;
	private bool deathScream;

	/*on screen debug
	private Text directionText;
	private Text speedText;
	private Text moveText;
	private Text groundedText;
    private Text distanceText;
    private Text crouchText;*/

	void Awake ()
	{
		
		kayaRB = GetComponent<Rigidbody> ();					//init player rigid body component\
		kayaCollider = GetComponent<CapsuleCollider> ();		//init player collider
		anim = GetComponent<Animator> ();						//init player animation component
		cam = Camera.main.transform;							//init main camera transform
		isoCam = Camera.main.GetComponent<IsometricCamera> ();	//init isoCam component of main camera
		startingMaterial = kayaCollider.material;				//init starting capsule collider material
		wilhelmDeath = GetComponent<AudioSource>();
	}

	void Start ()
	{
		/*
        //debug UI components
        speedText = GameObject.Find("/Canvas/Speed").GetComponent<Text>();
		directionText = GameObject.Find ("/Canvas/Direction").GetComponent<Text> ();
		moveText = GameObject.Find ("/Canvas/MoveVector").GetComponent<Text> ();
		groundedText = GameObject.Find ("/Canvas/Grounded").GetComponent<Text> ();
        distanceText = GameObject.Find("/Canvas/Distance").GetComponent<Text>();
        crouchText = GameObject.Find("/Canvas/Crouch").GetComponent<Text>();

        kayaRB = GetComponent<Rigidbody> ();					//init player rigid body component\
		kayaCollider = GetComponent<CapsuleCollider>();			//init player collider
		anim = GetComponent<Animator>();						//init player animation component
		cam = Camera.main.transform;							//init main camera transform
		isoCam = Camera.main.GetComponent<IsometricCamera>();	//init isoCam component of main camera
        */                                        

		//define avatarBones with rigid bodies in player game object

		avatarBones = gameObject.GetComponentsInChildren<Rigidbody> ();

		//set avatarBones to kinematic

		foreach (Rigidbody bone in avatarBones) {

			bone.isKinematic = true;
		}

		//reset player parent rigid body component to non kinematic and freeze rotations

		kayaRB.isKinematic = false;
		kayaRB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        
		//define layer mask to be complement of those selected for ignore

		mask = ~mask;

		//determine and define starting ground distance and define normal as up vector

		origDistToGround = kayaCollider.bounds.extents.y;
		distToGround = origDistToGround;
		groundNormal = Vector3.up;

		//set initial jump state to false
		jump = false;

		//determine capsule size for crouch collider animation
		crouch = false;
		capsuleHeight = kayaCollider.height;
		capsuleCenter = kayaCollider.center;

		//setup camera
		camIndex = 0;

		//set deathScream to false
		deathScream = false;

	}

	void OnCollisionEnter (Collision other)
	{
		//check to see if collision is with an obstacle in the scene
		if (other.gameObject.CompareTag ("Obstacle")) {
			print ("Hit Obstacle");
			ragDoll = true;
		}

		//check to see if collision is with platform
		if (other.gameObject.CompareTag ("Platform")) {

			//change to high friction collider to improve sticking to platform
			kayaCollider.material = platformMaterial;
		
		}

	}

	void OnCollisionExit(Collision other){

		//when exiting platform return to low riction colloider
		if (other.gameObject.CompareTag ("Platform")) {

			kayaCollider.material = startingMaterial;

		}	
	}

	void Update ()
	{
		currentBaseState = anim.GetCurrentAnimatorStateInfo (0); // set  currentState variable to the current state of the Base Layer of animation

		if (!jump) {
			
			jump = Input.GetButtonDown ("Jump");

			//play jump effects
			if(jump && currentBaseState.fullPathHash == moveAnimState && !anim.IsInTransition(0))
				Invoke("jumpEffects",0.1f);
		
		}			

		if (ragDoll)
			Invoke ("goForRagdoll", timeToRagdoll);					

		if (Input.GetKeyDown (KeyCode.C) | Input.GetButtonDown ("Crouch"))
			crouch = !crouch;

		if (Input.GetKeyDown (KeyCode.LeftBracket) | Input.GetButtonDown ("RotateViewLeft")) {         
			camIndex = camIndex - 1;
			if (camIndex < 0)
				camIndex = 3;
		}
		if (Input.GetKeyDown (KeyCode.RightBracket) | Input.GetButtonDown ("RotateViewRight")) {
			camIndex = camIndex + 1;
			if (camIndex > 3)
				camIndex = 0;
		}
		//update camera view
		isoCam.RotateCamera (camIndex);
	}

	void FixedUpdate ()
	{

		//get user input
		float h = Input.GetAxis ("Horizontal");
		float v = Input.GetAxis ("Vertical");

		//set forward and right move vectors for the player relative to the camera 
		mainCamForward = Vector3.Scale (cam.forward, new Vector3 (1.0f, 0.0f, 1.0f)).normalized;
		mainCamRight = cam.right;

		// calculate move direction to pass to character
		moveInput = v * mainCamForward + h * mainCamRight;

		//move the player game object
		Move (moveInput);       
	}

	public void Move (Vector3 move)
	{
	
		//normalize the move vector
		if (move.magnitude > 1.0f)
			move = Vector3.Normalize (move);

		//Debug.Log ("updated move vector: " + move);
		//on screen debug 
		//moveText.text = "Move: x = "+move.x+" y = "+move.y+" z = "+move.z;

		//convert world relative input move vector to local relative
		move = transform.InverseTransformDirection (move);
        
		//check for player ground condition
		groundInfo currentGround = playerGrounded ();
		isGrounded = currentGround.onGround;
		distToGround = currentGround.distance;
		groundNormal = currentGround.normal;
		landingPoint = currentGround.landing;

		//ragdoll death if fall distance is large
		if (distToGround > maxFallDistance) {
			ragDoll = true;
		}

		//determine local relative player movement if ground conditions are okay
		move = Vector3.ProjectOnPlane (move, groundNormal);
		forward = move.z;
		turn = Mathf.Atan2 (move.x, move.z);

		//update mecanim AC parameters
		//move allows for updating the AC using polar coordinates

		animSpeed = Vector3.SqrMagnitude (move); //transition between walk (0.0) and run (1.0)-magnitude not used to improve runtime
		animDirection = turn;			        //radians to rotate: +ve - turn right; -ve - turn left
		updateAC (animSpeed, animDirection, landingPoint);
		
		/*on screen debug for what is passed to mecanim
		speedText.text = "Speed: " + forward;
		directionText.text = "Direction: " + turn;
        groundedText.text = "Grounded: " + isGrounded;
        distanceText.text = "Distance to Ground: " + distToGround;*/
        	
	}
    
	// update parameters for mecanim AC
	public void updateAC (float s, float d, Vector3 land)
	{

		currentBaseState = anim.GetCurrentAnimatorStateInfo (0); // set  currentState variable to the current state of the Base Layer of animation
        
		//pivot if turn direction is greater than pi/4 in polar coordinates 
		if (Mathf.Abs (d) > 0.78) {
			anim.SetBool ("Pivot", true);
		} else {
			anim.SetBool ("Pivot", false);
		}
			
		anim.SetFloat ("Speed", s, 0.1f, Time.deltaTime);
		anim.SetFloat ("Direction", d, 0.1f, Time.deltaTime);       

		//crouch
		if (crouch) {
			anim.SetBool ("Crouch", true);
			kayaCollider.center = capsuleCenter / 2;
			kayaCollider.height = capsuleHeight / 2;
		} else {
			anim.SetBool ("Crouch", false);
			kayaCollider.center = capsuleCenter;
			kayaCollider.height = capsuleHeight;
		}

		//standard jump
		if (isGrounded && currentBaseState.fullPathHash == moveAnimState) {
			if (jump) {
				
				anim.SetBool ("Jump", true);

				//footParticle.GetComponent<ParticleSystem> ().Play ();
				//footParticle.GetComponent<AudioSource> ().Play ();

				//Vector3			 jumpTarget = Vector3.Lerp(transform.position, transform.position + new Vector3(1,2,1) * jumpPower, jumpSmoothing);
				//Debug.Log("position: " + transform.position);
				//Debug.Log("jumpTarget: " + jumpTarget);

				//add jump power beyond root motion - cause camera follow to jump as well
				//transform.Translate(Vector3.up*Time.deltaTime*jumpPower);

				//float step = jumpSmoothing * Time.deltaTime;
				//Vector3 targetPos = transform.position + (new Vector3(1,2,1) * jumpPower); - calc target position needs work
				//transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
			}
                
		}
        // currently in jump state
        else if (currentBaseState.fullPathHash == jumpAnimState) {
			//not transitioning
			if (!anim.IsInTransition (0)) {
				transform.Translate (Vector3.forward.x * Time.deltaTime * jumpPower / 2, 
					Time.deltaTime * jumpPower,
					Vector3.forward.z * Time.deltaTime * jumpPower / 2,
					Space.Self);
				//Debug.Log("in jump state");
				//collider animation curve here

				if (distToGround > 2.0f) {
					//match target with animation; start jump - 0.02f. f; end jump - 0.76f
					//Debug.Log("match target required");
					anim.MatchTarget (land, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask (new Vector3 (0, 1, 0), 0), 0.02f, 0.76f);
				}
			}
		}
		if (jump)
			Invoke ("stopJump", 0.1f);
	}

	/*public void goForJump (bool j){

		anim.SetBool ("Jump", j);
		Invoke ("stopJump", 0.1f);
	}*/

	//invoked stop jump conditions
	void stopJump ()
	{

		anim.SetBool ("Jump", false);
		jump = false;
	}

	//invoked jump effects
	void jumpEffects(){
	
		footParticle.GetComponent<ParticleSystem> ().Play ();
		footParticle.GetComponent<AudioSource> ().Play ();
	}

	//transition from anim to ragdoll physics
	public void goForRagdoll ()
	{
		bool deathSound = false;

		//set collider material to high friction material
		kayaCollider.material = (PhysicMaterial)Resources.Load("PhysicMaterials/None");

		//disable animation controller
		anim.enabled = false;

		//set avatar Bones to non kinematic objects
		foreach (Rigidbody bone in avatarBones) {
			bone.isKinematic = false;
		}

		ragDoll = false;

		//ragdoll effects
		if (!deathScream) {

			wilhelmDeath.Play ();
			deathScream = true;
		}

		bloodParticle.GetComponent<ParticleSystem> ().Play ();
		StartCoroutine (reloadScene ());
	}

	public void setRagdoll ()
	{
		if (!ragDoll) {
			ragDoll = true;
		}
            
	}

	groundInfo playerGrounded ()
	{

		groundInfo currentGroundInfo;
		bool gnd;
		float dist = 0.0f;
		Vector3 norm = Vector3.up;
		Vector3 land = transform.position;

		Vector3 start = kayaCollider.bounds.center;
		Vector3 end = new Vector3 (kayaCollider.bounds.center.x, kayaCollider.bounds.min.y - groundHeight, kayaCollider.bounds.center.z);
		float rad = kayaCollider.radius;

		gnd = Physics.CheckCapsule (start, end, rad, mask);

		if (!gnd) {
			
			//Debug.Log("Airborn snitch!");

			RaycastHit hit;

			if (Physics.Raycast (kayaCollider.bounds.center, -Vector3.up, out hit, 100f, mask)) {
				dist = hit.distance;
				norm = hit.normal;
				land = hit.point;
			}
		} else {
			
			dist = origDistToGround;
			land = transform.position;

		}

		currentGroundInfo.onGround = gnd;
		currentGroundInfo.distance = dist;
		currentGroundInfo.normal = norm;    //returns up vector by default
		currentGroundInfo.landing = land;   //returns player position by default

		return currentGroundInfo;
	}

	IEnumerator reloadScene ()
	{
		yield return new WaitForSeconds (3);
		Scene scene = SceneManager.GetActiveScene ();
		SceneManager.LoadScene (scene.name);
	}
}
