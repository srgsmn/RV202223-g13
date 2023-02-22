using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairliftController : MonoBehaviour
{
    public GameObject Player = null;
    public GameObject LeftArm;
    public GameObject RightArm;
    private float _armsRot = 0f;

    private enum stairlift_state { open_start, closing_arms, moving_forward, opening_arms, moving_backward, open_end};
    private stairlift_state state = stairlift_state.open_start;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _endPosition = Vector3.zero;
    private bool forward = true;

    private bool _playerInside = false;
    private Collider _collider;
    private Collider _playerCollider;
    private bool _isMoving;
    private float _movement = 0f;

    private bool _spacePressed;

    
    // Start is called before the first frame update
    void Start()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Player == null)
        {
            Player = collision.collider.gameObject;
            _playerCollider = Player.GetComponent<Collider>();
        }
        _playerInside = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        _playerInside = false;
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case stairlift_state.open_start:
                if (_playerInside && _spacePressed)
                {
                    _spacePressed = false;
                    _movement = 0f;
                    _armsRot = 0f;
                    state = stairlift_state.closing_arms;
                    forward = true;
                }
                break;

            case stairlift_state.open_end:
                if (_playerInside && _spacePressed)
                {
                    _spacePressed = false;
                    _movement = 0f;
                    _armsRot = 0f;
                    state = stairlift_state.closing_arms;
                    forward = false;
                }
                break;

            case stairlift_state.closing_arms:
                if (_armsRot <= 90f)
                {
                    float delta = 30f * Time.deltaTime;
                    _armsRot += delta;
                    LeftArm.transform.Rotate(delta, 0, 0);
                    RightArm.transform.Rotate(delta, 0, 0);
                }
                else
                {
                    state = (forward) ? stairlift_state.moving_forward : stairlift_state.moving_backward;
                }
                break;

            case stairlift_state.opening_arms:
                if(_armsRot >= 0f)
                {
                    float delta = 30f * Time.deltaTime;
                    _armsRot -= delta;
                    LeftArm.transform.Rotate(-delta, 0, 0);
                    RightArm.transform.Rotate(-delta, 0, 0);
                } else
                {
                    state = (forward) ? stairlift_state.open_end : stairlift_state.open_start;
                }
                break;


            case stairlift_state.moving_forward:
                if (gameObject.transform.position != _endPosition)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _endPosition, 5 * Time.deltaTime);
                    Player.transform.position.Set(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z + Player.transform.position.z);
                } else
                {
                    state = stairlift_state.opening_arms;
                }
                break;

            case stairlift_state.moving_backward:
                if (gameObject.transform.position != _startPosition)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _startPosition, 30 * Time.deltaTime);
                }
                else
                {
                    state = stairlift_state.opening_arms;
                }
                break;

            default: break;
        }
    }

    void PressSpace()
    {
        if(state == stairlift_state.open_start || state == stairlift_state.open_end) _spacePressed = true;
    }

    public void SetStart(Vector3 position)
    {
        _startPosition = position;
    }
    public void SetEnd(Vector3 position)
    {
        _endPosition = position;
    }

    private void OnEnable()
    {

        InputManager.OnSelection += PressSpace;
    }

    private void OnDisable()
    {

        InputManager.OnSelection -= PressSpace;
    }

}
