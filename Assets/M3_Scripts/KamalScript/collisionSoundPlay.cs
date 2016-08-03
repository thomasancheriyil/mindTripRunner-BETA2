using UnityEngine;
using System.Collections;

public class collisionSoundPlay : MonoBehaviour {
	private timeManager gameTimer;
	AudioSource myaudio;

	// Use this for initialization
	void Start () {
		gameTimer = GameObject.Find ("TimeManager").GetComponent<timeManager>();
		myaudio = GameObject.Find("ballAudio").GetComponent<AudioSource>();

	}
	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag ("Player")) {
			myaudio.Play ();
			gameTimer.timeLeft -= gameTimer.penalty;
		}
		Destroy (this.gameObject,1);
	}
	// Update is called once per frame
	void Update () {

	}
}
