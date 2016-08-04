using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class popUpMoveFade : MonoBehaviour {
	// Use this for initialization
	Text a;
	void Start () {
	}
	// Update is called once per frame
	void Update () {
		transform.Translate(Vector3.down * 100* Time.deltaTime, Space.World);
		transform.Translate(Vector3.left * 140* Time.deltaTime, Space.World);
	}
}
