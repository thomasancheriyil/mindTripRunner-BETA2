using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class endSound : MonoBehaviour {
	public float timeLag = 1.0F;
	AudioSource myaudio;
	private AudioSource[] allAudioSources;
	private bool fg =true;
	public bool winGame = false;
	// Use this for initialization
	void Start () {
		myaudio = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
		if (!fg && !myaudio.isPlaying) {
			SceneManager.LoadScene(0);
		}
	}

	void OnCollisionEnter(Collision collision) {
		if (fg) {
			StopAllAudio ();
			fg = false;
			myaudio.Play ();
			winGame = true;
		}
	}
	void StopAllAudio() {
		allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
		foreach( AudioSource audioS in allAudioSources) {
			audioS.Stop();
		}
	}

}
