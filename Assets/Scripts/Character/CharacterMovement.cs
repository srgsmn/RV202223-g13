using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //public Camera PlayerCamera;
    public GameObject Floor;
    public float WalkSpeed = 3f;
    public float TurnSpeed = 120f;
    public float LookSpeed = 160f;
    //public Vector3 _forward = new Vector3(0,0,1);
    public Transform MassCenter;
    private float _startSpeed;
    private float _startAccel;
    private float _steepDist = 0.0001f;
    private Rigidbody _rigidbody;
    private Vector2 _localMovement;
    
    private bool _reportDone=false;
    private bool _movingOnY=false;

    public GameObject Target;

    public delegate void HasMoved(float distance);
    public static event HasMoved OnMovement;

    public delegate void HasCollided(Collision collision);
    public static event HasCollided OnPlayerCollision;

    public delegate void TargetReached();
    public static event TargetReached OnTargetReached;

    // Start is called before the first frame update
    void Start()
    {
        _startSpeed = 0f;
        _startAccel = 10f;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass=MassCenter.position - this.transform.position;
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.freezeRotation=true;

        //int curr_mode = gameObject.GetComponent<FurnitureSelection>().GetCurrentMode(); // valid
    }

    // Update is called once per frame
    private void Update()
    {
        // enum e_mode { mode_navigation, mode_selection, mode_move };
        if ((this.transform.position-Target.transform.position).magnitude<=1.0f && !_reportDone){
            Debug.Log("Sono arrivato");
            OnTargetReached?.Invoke();
            _reportDone=true;
            GameManager.Instance.EndsGame();
        }
       //_rigidbody.angularVelocity = Vector3.zero;

        
    }
    public void ApplyMovement(Vector2 movement){
        _localMovement=movement;
        _movingOnY=true;
    }

    void OnCollisionEnter(Collision collision){
        OnPlayerCollision?.Invoke(collision);
        if (collision.gameObject.name.ToLower().IndexOf("ramp")!=-1 && collision.gameObject.transform.parent != null)
        {
            foreach(Rigidbody r in collision.gameObject.transform.parent.gameObject.GetComponentsInChildren<Rigidbody>()){
                r.isKinematic=true;
            }
        }
    }

    void FixedUpdate()
    {
        if (_movingOnY){
            if (_localMovement.y > 0.5f)
            {
                if (_startSpeed <= 0) _startSpeed = 0;
                if (_startSpeed < WalkSpeed)
                {
                    //Debug.Log("Sto andando avanti");
                    //transform.Translate(gameObject.transform.forward * _startSpeed * Time.deltaTime);
                    _rigidbody.velocity = -_startSpeed * transform.forward; 
                    _startSpeed += _startAccel * Time.deltaTime;
                }
                //else transform.Translate(gameObject.transform.forward * WalkSpeed * Time.deltaTime);
                else _rigidbody.velocity = -WalkSpeed * transform.forward;
                RaycastHit raycastHit;
                if(Physics.Raycast(gameObject.transform.position, gameObject.transform.forward - Vector3.down, out raycastHit, 2f))
                {
                    float f = raycastHit.point.y - (gameObject.transform.position.y - GetComponent<BoxCollider>().bounds.size.y * 0.5f);
                    if (raycastHit.collider.gameObject.name.ToLower().IndexOf("ramp") != -1 && f > 0 && f <= _steepDist)
                    {
                        _rigidbody.velocity += Vector3.up * f;
                    }
                }
            }
            else if (_localMovement.y < -0.5f)
            {
                if (_startSpeed >= 0) _startSpeed = 0;
                if (_startSpeed > -WalkSpeed)
                {
                    //transform.Translate(gameObject.transform.forward * _startSpeed * Time.deltaTime);
                    _rigidbody.velocity = -_startSpeed * transform.forward;
                    _startSpeed -= _startAccel * Time.deltaTime;
                }
                //else transform.Translate(-gameObject.transform.forward * WalkSpeed * Time.deltaTime);
                else _rigidbody.velocity = WalkSpeed * transform.forward;
                RaycastHit raycastHit;
                if (Physics.Raycast(gameObject.transform.position, -gameObject.transform.forward - Vector3.down, out raycastHit, 2f))
                {
                    float f = raycastHit.point.y - (gameObject.transform.position.y - GetComponent<BoxCollider>().bounds.size.y * 0.5f);
                    if (raycastHit.collider.gameObject.name.ToLower().IndexOf("ramp") != -1 && f > 0 && f <= _steepDist)
                    {
                        _rigidbody.velocity += Vector3.up * f;
                    }
                }
            }
            else
            {
                //_startSpeed = 0f;
                _rigidbody.velocity = Vector3.zero;
                _movingOnY=false;
            }
        }

        if (_localMovement.x > 0.5)
        {
            transform.Rotate(Vector3.up, TurnSpeed * Time.deltaTime);
            //_rigidbody.angularVelocity = TurnSpeed *  Vector3.up;
        }
        else if (_localMovement.x < -0.5)
        {
            transform.Rotate(Vector3.up, -TurnSpeed * Time.deltaTime);
            //_rigidbody.angularVelocity = -TurnSpeed * Vector3.up;
        }
        else
        {
            //_rigidbody.angularVelocity = Vector3.zero;
        }
        if(Mathf.Abs(_localMovement.y) > 0f)
        {
            OnMovement?.Invoke((_rigidbody.velocity * Time.deltaTime).magnitude);
        }
    }

    


    private void OnEnable()
    {
        InputManager.OnMovementInput += ApplyMovement;
    }

    private void OnDisable()
    {
        InputManager.OnMovementInput -= ApplyMovement;

    }

    // FixedUpdate is called every fixed rate framerate frame
    /*void FixedUpdate()
    {
        
        
        if (Input.GetKey(KeyCode.W))
        {
            if (_startSpeed <= 0) _startSpeed = 0;
            if (_startSpeed < WalkSpeed)
            {
                //transform.Translate(gameObject.transform.forward * _startSpeed * Time.deltaTime);
                _rigidbody.velocity = _startSpeed * transform.forward;
                _startSpeed += _startAccel * Time.deltaTime;
            }
            //else transform.Translate(gameObject.transform.forward * WalkSpeed * Time.deltaTime);
            else _rigidbody.velocity = WalkSpeed * transform.forward;
        } else if (Input.GetKey(KeyCode.S))
        {
            if (_startSpeed >= 0) _startSpeed = 0;
            if (_startSpeed > -WalkSpeed)
            {
                //transform.Translate(gameObject.transform.forward * _startSpeed * Time.deltaTime);
                _rigidbody.velocity = _startSpeed * transform.forward;
                _startSpeed -= _startAccel * Time.deltaTime;
            }
            //else transform.Translate(-gameObject.transform.forward * WalkSpeed * Time.deltaTime);
            else _rigidbody.velocity = -WalkSpeed * transform.forward;
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
    }*/
}
