using UnityEngine;
using System.Collections;

public class PolishKey : MonoBehaviour {

	public GameObject doorObject;
	GameObject redLight;
	GameObject greenLight;

	public float spinSpeed = 1.0f;

	void Start () {

		redLight = GameObject.FindGameObjectWithTag("RedLight");
		greenLight = GameObject.FindGameObjectWithTag("GreenLight");
	}

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up*spinSpeed*Time.deltaTime);
	}

	void OnTriggerEnter (Collider col) {

		if (col.gameObject.CompareTag ("Player")) {

			redLight.GetComponent<Light>().enabled = false;
			greenLight.GetComponent<Light>().enabled = true;

			//this.GetComponent<Renderer>().enabled = false;
			//this.GetComponent<Light>().enabled = false;

			doorObject.GetComponent<doorScript>().isUnlocked = true;
			Destroy(this.gameObject);			
		}

	}

}

