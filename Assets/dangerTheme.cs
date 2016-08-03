using UnityEngine;
using System.Collections;

public class dangerTheme : MonoBehaviour {
	public GameObject player;
	public float dangerRadius;
	AudioSource myaudio;
	AudioSource bg;
	Vector3 pp;
	float dis;
	// Use this for initialization
	void Start () {
		myaudio = this.GetComponent<AudioSource>();
		bg = GameObject.Find ("bgSound").GetComponent<AudioSource>(); 
	}

//	 Update is called once per frame
		void Update () {
			pp = player.transform.position;
			dis = (pp - transform.position).magnitude;
			if (dis < dangerRadius) {
			if (bg.isPlaying) {
				bg.volume -= 0.5F*Time.deltaTime;
				if (bg.volume < .01) {
					bg.Stop ();
				}
			}
				if (!myaudio.isPlaying) {
					myaudio.Play ();
				}
			}
			else {
				if (myaudio.isPlaying) {
				myaudio.volume -= 0.1F*Time.deltaTime;
				if (myaudio.volume < .05) {
					myaudio.Stop ();
				}

				}
			else{
				if (!bg.isPlaying) {
					bg.Play ();
					bg.volume = 1F;
					print ("Here");
				}
			}
		}
}
}