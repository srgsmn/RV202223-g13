using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairliftController : MonoBehaviour
{
    public GameObject Player = null;
    public GameObject LeftArm;
    public GameObject RightArm;
    public float ArmsRotation = 0f;
    public bool isPlayerInside = false;
   
    private enum stairlift_state { open_start, closing_arms, moving_forward, opening_arms, moving_backward, open_end};
    private stairlift_state state = stairlift_state.open_start;
    private Vector3 _startPosition = Vector3.zero;
    private Vector3 _endPosition = Vector3.zero;
    private bool forward = true;
    private bool _playerAssigned = false;
    private bool _spacePressed = false;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        if (_playerAssigned)
        {
            if (!isPlayerInside && asVector2(Player.transform.position - gameObject.transform.position).magnitude <= 0.5)
            {
                isPlayerInside = true;
            }
            else if (asVector2(Player.transform.position - gameObject.transform.position).magnitude > 0.5)
            {
                isPlayerInside = false;
            }
        }
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case stairlift_state.open_start:
                //if (isPlayerInside && _spacePressed)
                if (isPlayerInside && Input.GetKeyDown(KeyCode.Space))
                {
                    Debug.Log("Space pressed: closing arms");
                    //_spacePressed = false;
                    ArmsRotation = 0f;
                    state = stairlift_state.closing_arms;
                    forward = true;
                }
                break;

            case stairlift_state.open_end:
                if (isPlayerInside && _spacePressed)
                {
                    Debug.Log("Space pressed: closing arms");
                    _spacePressed = false;
                    ArmsRotation = 0f;
                    state = stairlift_state.closing_arms;
                    forward = false;
                }
                break;

            case stairlift_state.closing_arms:
                if (ArmsRotation <= 90f)
                {
                    float delta = 30f * Time.deltaTime;
                    ArmsRotation += delta;
                    LeftArm.transform.Rotate(delta, 0, 0, Space.Self);
                    RightArm.transform.Rotate(delta, 0, 0, Space.Self);
                }
                else
                {
                    Player.transform.SetParent(gameObject.transform, true);
                    Player.GetComponent<Rigidbody>().isKinematic = true;
                    state = (forward) ? stairlift_state.moving_forward : stairlift_state.moving_backward;
                }
                break;

            case stairlift_state.opening_arms:
                if(ArmsRotation >= 0f)
                {
                    float delta = 30f * Time.deltaTime;
                    ArmsRotation -= delta;
                    LeftArm.transform.Rotate(-delta, 0, 0, Space.Self);
                    RightArm.transform.Rotate(-delta, 0, 0, Space.Self);
                } else
                {
                    Player.transform.SetParent(null);
                    Player.GetComponent<Rigidbody>().isKinematic = false;
                    state = (forward) ? stairlift_state.open_end : stairlift_state.open_start;
                }
                break;


            case stairlift_state.moving_forward:
                if (gameObject.transform.position != _endPosition)
                {
                    gameObject.transform.position = Vector3.MoveTowards( gameObject.transform.position, _endPosition, 2f * Time.deltaTime);
                } else
                {
                    state = stairlift_state.opening_arms;
                }
                break;

            case stairlift_state.moving_backward:
                if (gameObject.transform.position != _startPosition)
                {
                    gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, _startPosition, 2f * Time.deltaTime);
                }
                else
                {
                    state = stairlift_state.opening_arms;
                }
                break;

            default: break;
        }
    }

    Vector2 asVector2(Vector3 v)
    {
        return new Vector2(v.x, v.z);
    }

    void PressSpace()
    {
        _spacePressed = true;
    }

    public void SetStart(Vector3 position)
    {
        _startPosition = position;
    }
    public void SetEnd(Vector3 position)
    {
        _endPosition = position;
    }

    public void SetPlayer(GameObject player)
    {
        Player = player;
        _playerAssigned = true;
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
