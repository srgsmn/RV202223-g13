using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //public Camera PlayerCamera;
    public float WalkSpeed = 3f;
    public float TurnSpeed = 120f;
    public float LookSpeed = 160f;
    //public Vector3 _forward = new Vector3(0,0,1);
    public Transform MassCenter;
    private float _startSpeed;
    private float _startAccel;
    private Rigidbody _rigidbody;
    //private Vector2 camera_rot = new Vector2();

    // Start is called before the first frame update
    void Start()
    {
        _startSpeed = 0f;
        _startAccel = 3f;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass=MassCenter.position - this.transform.position;
        _rigidbody.velocity = Vector3.zero;

        //int curr_mode = gameObject.GetComponent<FurnitureSelection>().GetCurrentMode(); // valid
    }

    // Update is called once per frame
    private void Update()
    {
        // enum e_mode { mode_navigation, mode_selection, mode_move }; 
        
    }

    // FixedUpdate is called every fixed rate framerate frame
    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (_startSpeed <= 0) _startSpeed = 0;
            if (_startSpeed < WalkSpeed)
            {
                //transform.Translate(gameObject.transform.forward * _startSpeed * Time.deltaTime);
                _rigidbody.velocity = _startSpeed * (-transform.forward);
                _startSpeed += _startAccel * Time.deltaTime;
            }
            //else transform.Translate(gameObject.transform.forward * WalkSpeed * Time.deltaTime);
            else _rigidbody.velocity = WalkSpeed * (-transform.forward);
        } else if (Input.GetKey(KeyCode.S))
        {
            if (_startSpeed >= 0) _startSpeed = 0;
            if (_startSpeed > -WalkSpeed)
            {
                //transform.Translate(gameObject.transform.forward * _startSpeed * Time.deltaTime);
                _rigidbody.velocity = _startSpeed * (-transform.forward);
                _startSpeed -= _startAccel * Time.deltaTime;
            }
            //else transform.Translate(-gameObject.transform.forward * WalkSpeed * Time.deltaTime);
            else _rigidbody.velocity = -WalkSpeed * (-transform.forward);
        } else
        {
            //_startSpeed = 0f;
            _rigidbody.velocity = Vector3.zero;
        }

        if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime);
            //_rigidbody.angularVelocity = TurnSpeed *  Vector3.up;
        } else if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -TurnSpeed * Time.deltaTime);
            //_rigidbody.angularVelocity = -TurnSpeed * Vector3.up;
        } else
        {
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }
}
