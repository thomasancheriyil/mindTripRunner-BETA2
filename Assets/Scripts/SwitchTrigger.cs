using UnityEngine;
using System.Collections;

public class SwitchTrigger : MonoBehaviour {

    public GameObject movingPlatform;
    private MovingPlatformTriggered movPlat;
    public Material mainColor;
    public Material highlightColor;

    void Start()
    {
        this.transform.GetComponent<Renderer>().material = mainColor;
        movPlat = movingPlatform.GetComponent<MovingPlatformTriggered>();
    }
    void OnTriggerEnter(Collider col)
    {
        this.transform.GetComponent<Renderer>().material = highlightColor;
        movPlat.movePlatform = true;
    }

    void OnTriggerStay(Collider col)
    {
        movPlat.movePlatform = true;
    }
    void OnTriggerExit(Collider col)
    {
        this.transform.GetComponent<Renderer>().material = mainColor;
        movPlat.movePlatform = false;
    }
}
