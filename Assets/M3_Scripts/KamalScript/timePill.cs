using UnityEngine;
using System.Collections;

public class timePill : MonoBehaviour {
	private timeManager gameTimer;
	AudioSource myaudio;
	private GameObject mylight;

	// Use this for initialization
	void Start () {
		gameTimer = GameObject.Find ("TimeManager").GetComponent<timeManager>();
		myaudio = GameObject.Find("pillAudio").GetComponent<AudioSource>();
		mylight = GameObject.Find ("pillLight");
	
	}
	void OnCollisionEnter(Collision other){
		if (other.gameObject.CompareTag ("Player")) {
			gameTimer.timeLeft += gameTimer.bonus;
		}
		myaudio.Play();
		Destroy (this.gameObject);
		Destroy (mylight);
	}
	// Update is called once per frame
	void Update () {
	
	}
}
