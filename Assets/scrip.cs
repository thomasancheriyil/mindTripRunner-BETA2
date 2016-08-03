using UnityEngine;
using System.Collections;

public class scrip : MonoBehaviour {
	public Transform cent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.RotateAround(cent.position, cent.up, 10 * Time.deltaTime);
	}
}
