using UnityEngine;
using System.Collections;

public class SwitchPlatformAppear : MonoBehaviour {

    public Transform movingPlatform;
    public Transform hiddenPosition;
    public Transform mainPosition;
    public Vector3 newPosition;
    public bool platformAppear = false;
    public float smooth = 1;

    public float appearDuration = 3;

    private bool animate = false;
    
    // Use this for initialization
    void Start()
    {
        platformAppear = false;
        newPosition = hiddenPosition.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(platformAppear)
        {
            Invoke("makePlatformAppear", 0);
            platformAppear = false;

        }
        movingPlatform.position = Vector3.Lerp(movingPlatform.position, newPosition, smooth * Time.deltaTime);
        /*
        if(movingPlatform.position == newPosition)
        {
            Invoke("hidePlatform", appearDuration);
        } */
    }

    void makePlatformAppear()
    {
        newPosition = mainPosition.position;
        platformAppear = false;
        Invoke("hidePlatform", appearDuration);
    }

    void hidePlatform()
    {
        newPosition = hiddenPosition.position;
        platformAppear = false;
    }

}
