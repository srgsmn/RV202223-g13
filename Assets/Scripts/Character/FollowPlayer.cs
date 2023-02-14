using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public float TurnSpeedX = 10f;
    public float TurnSpeedY = 15f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    // Start is called before the first frame update
    void Start()
    {
        transform.localEulerAngles = new Vector3(0f, 180f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        rotationX = Mathf.Clamp(rotationX + Input.GetAxis("Mouse Y") * TurnSpeedX, -15f, 45f);
        rotationY = Mathf.Clamp(rotationY + Input.GetAxis("Mouse X") * TurnSpeedY, -60f, 60f);

        transform.localEulerAngles = new Vector3(rotationX, rotationY + 15f, -0.1f * rotationY);
    }
}
