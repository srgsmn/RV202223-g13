using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampController : MonoBehaviour
{
    public GameObject RampGetOn;
    public GameObject RampGetOff;

    private Rigidbody _rigidbody;
    private bool _isKinematic = false;
    private float _timeFromPosition = 0f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.ToLower().IndexOf("avatar") != -1)
        {
            _rigidbody.isKinematic = true;
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = true;
            }
            _isKinematic = true;
        }
    }
}
