using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    [SerializeField] Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float angle;
        //angle=Mathf.Rad2Deg*(Mathf.Acos((Vector3.Dot(transform.forward,cam.transform.position-transform.position))/(((transform.forward.magnitude)*((cam.transform.position-transform.position).magnitude)))));
        //transform.LookAt(transform.position + cam.transform.forward,Vector3.up);
        Vector3 direction=cam.transform.position-transform.position;
        Vector3 horizontalPlane=new Vector3(0f,1.0f,0f);
        Vector3 flatDirection=Vector3.Normalize(Vector3.ProjectOnPlane(direction,horizontalPlane));
        angle=Vector3.Angle(-transform.forward,flatDirection);
        transform.Rotate(0.0f,angle,0.0f,Space.World);
    }
}
