using UnityEngine;
using System.Collections;

public class AppearTrigger : MonoBehaviour {

    public GameObject hiddenPlatform;
    private SwitchPlatformAppear swPlatAppear;
    public Material mainColor;
    public Material highlightColor;

    void Start()
    {

        this.transform.GetComponent<Renderer>().material = mainColor;
        swPlatAppear = hiddenPlatform.GetComponent<SwitchPlatformAppear>();
    }
    void OnTriggerEnter(Collider col)
    {
        this.transform.GetComponent<Renderer>().material = highlightColor;
        swPlatAppear.platformAppear = true;
    }

    void OnTriggerExit(Collider col)
    {
        this.transform.GetComponent<Renderer>().material = mainColor;
    }

}
