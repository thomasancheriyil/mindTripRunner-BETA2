using UnityEngine;
using System.Collections;

public class MenuCamControl : MonoBehaviour {

    public Transform currentMount;
    public float speedFactor = 0.1f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = Vector3.Lerp(this.transform.position, currentMount.position, speedFactor);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, currentMount.rotation, speedFactor);
    }

    public void setMount(Transform newMount)
    {
        currentMount = newMount;
    }
}
