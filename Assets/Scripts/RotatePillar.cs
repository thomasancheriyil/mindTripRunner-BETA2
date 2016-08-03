using UnityEngine;
using System.Collections;

public class RotatePillar : MonoBehaviour {

    public GameObject rotatePoint;
    public float speed = 10f;

    // 0 is for Up rotation, 1 is for Right Rotation.
    public float axis = 0;
    void Update()
    {
        if (axis == 0)
        {
            transform.RotateAround(rotatePoint.transform.position, Vector3.up, speed * Time.deltaTime);
        }
        if (axis == 1)
        {
            transform.RotateAround(rotatePoint.transform.position, Vector3.right, speed * Time.deltaTime);
        }

    }
}
