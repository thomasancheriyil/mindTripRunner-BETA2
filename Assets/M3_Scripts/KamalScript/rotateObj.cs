using UnityEngine;
using System.Collections;

public class rotateObj : MonoBehaviour {
	private float rotationSpeed;
	private Transform player;
	private float dp;
	private float cone_view;
	private float rotAngle;
	public bool pur;
	private Vector3 fix_point;
	private GameObject cannon;
	private initialCannon param;
	AudioSource myaudio;

	//public Color c1 = Color.blue;
	//public Color c2 = Color.black;
	//public int lengthOfLineRenderer = 2;
	private Vector3 canCenter;

	void Start () {
		cannon = GameObject.Find ("cannonBase");
		param = GameObject.Find ("cannon").GetComponent<initialCannon>();
		myaudio = GetComponent<AudioSource> ();
		fix_point = cannon.transform.position;
		fix_point.y = transform.position.y;
		//LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
		//lineRenderer.material = new Material(Shader.Find("Unlit/Texture"));
		//lineRenderer.SetColors(c1, c1);
		//lineRenderer.SetWidth(0.1F, 0.1F);
		canCenter.Set (fix_point.x, 0.0F ,fix_point.z);
		dp = param.distanceThreshold;
		cone_view = param.coneView;
		rotationSpeed = param.rotationSpeed;
		player = param.target;


	}
	
	// Update is called once per frame
	void Update () {
		//LineRenderer lineRenderer = GetComponent<LineRenderer>();
		Vector3 mp = player.position;
		Vector3 p2p = new Vector3 (mp.x - canCenter.x,0.0F, mp.z - canCenter.z);
		Vector3 _aim = transform.up;
		Vector3 aim = new Vector3 (_aim.x,0.0F, _aim.z);
		aim.Normalize ();
		Vector3 pos1 = new Vector3(canCenter.x,0.0F,canCenter.z);

		float cone = Vector3.Angle (aim, p2p);
		if (p2p.magnitude < dp && cone < cone_view) {
			Rigidbody T = player.GetComponent<Rigidbody>();
			Vector3 new_point;
			if (T.velocity.magnitude < .05F) {
				new_point = player.position;
			} else {
				new_point = player.position + T.velocity * Time.deltaTime;
			}
			Vector3 _predicted = new_point - canCenter;
			Vector3 predicted = new Vector3(_predicted.x, 0.0F ,_predicted.z);
			predicted.Normalize ();
			Vector3 pos2 = pos1 + 10 * predicted;
			//lineRenderer.SetPosition(0, pos1);
			//lineRenderer.SetPosition(1, pos2);
			float tmp = Vector3.Angle (predicted, aim);

			rotAngle = Mathf.Min (rotationSpeed * Time.deltaTime, tmp);

			float sig = predicted.x * aim.z - predicted.z * aim.x;
			if (sig < 0) {
				rotAngle = -rotAngle;
			}
			if (rotAngle < .1) {
				pur = true;
			} else {
				pur = false;
			}
			if (!myaudio.isPlaying) {
				myaudio.Play ();
			}
		} else {
			pur = false;
			rotAngle = 2*rotationSpeed * Time.deltaTime;
//			if (p2p.magnitude > dp) {
//				myaudio.Stop ();
//			} else {
//				if (!myaudio.isPlaying) {
//					myaudio.Play ();
//				}
	//		}
			myaudio.Stop();
		}
		transform.RotateAround(fix_point, Vector3.up, rotAngle);
	}
}
