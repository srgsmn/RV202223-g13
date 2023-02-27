using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampController : MonoBehaviour
{
    public GameObject RampGetOn;
    public GameObject RampGetOff;

    public void Place()
    {
        GetComponent<Rigidbody>().isKinematic = false;
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = false;
        }
    }
}
