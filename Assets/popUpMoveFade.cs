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
		transform.Translate(Vector3.up * 50* Time.deltaTime, Space.World);
		transform.Translate(Vector3.right * 50* Time.deltaTime, Space.World);
	}
}
