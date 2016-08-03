using UnityEngine;
using System.Collections;

public class moviePlay : MonoBehaviour {
	public MovieTexture movTexture;
	void Start() {
		GetComponent<Renderer>().material.mainTexture = movTexture;
		movTexture.loop = true;
		movTexture.Play();
	}
	void update(){
		if (!movTexture.isPlaying) {
			movTexture.Play ();
		}

	}
}