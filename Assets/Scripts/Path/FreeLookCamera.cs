using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// A simple free camera to be added to a Unity game object.
/// 
/// Keys:
///	wasd / arrows	- movement
///	q/e 			- up/down (local space)
///	r/f 			- up/down (world space)
///	pageup/pagedown	- up/down (world space)
///	hold shift		- enable fast movement mode
///	right mouse  	- enable free look
///	mouse			- free look / rotation
///     
/// </summary>
public class FreeLookCamera : MonoBehaviour
{
    /// <summary>
    /// Normal speed of camera movement.
    /// </summary>
    public float movementSpeed = 10f;
    /// <summary>
    /// Speed of camera movement when shift is held down,
    /// </summary>
    public float fastMovementSpeed = 30f;

    /// <summary>
    /// Sensitivity for free look.
    /// </summary>
    public float freeLookSensitivity = 3f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel.
    /// </summary>
    public float zoomSensitivity = 10f;

    /// <summary>
    /// Amount to zoom the camera when using the mouse wheel (fast mode).
    /// </summary>
    public float fastZoomSensitivity = 50f;

    /// <summary>
    /// Set to true when free looking (on right mouse button).
    /// </summary>
    private bool looking = false;

    private Vector2 _localTranslate;
    private Vector2 _localRotate;
    private float _localLift;
    private float _localZoom;

    void ApplyTranslation(Vector2 delta)
    {
        _localTranslate = delta;
    }
    void ApplyRotation(Vector2 delta)
    {
        _localRotate = delta;
    }
    void ApplyLifting(float delta)
    {
        _localLift = delta;
    }

    void ApplyZoom(float delta)
    {
        _localZoom = delta;
    }

    void Update()
    {
        //var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
        //var movementSpeed = fastMode ? this.fastMovementSpeed : this.movementSpeed;
        var movementSpeed = this.movementSpeed;

        //if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        if (_localTranslate.x < -0.5)
        {
            transform.position = transform.position + (-transform.right * movementSpeed * Time.deltaTime);
        }

        if (_localTranslate.x > 0.5)
        {
            transform.position = transform.position + (transform.right * movementSpeed * Time.deltaTime);
        }

        if (_localTranslate.y > 0.5)
        {
            transform.position = transform.position + (transform.forward * movementSpeed * Time.deltaTime);
        }

        if (_localTranslate.y < -0.5)
        {
            transform.position = transform.position + (-transform.forward * movementSpeed * Time.deltaTime);
        }

        if (_localLift > 0.5)
        {
            transform.position = transform.position + (transform.up * movementSpeed * Time.deltaTime);
        }

        if (_localLift < -0.5)
        {
            transform.position = transform.position + (-transform.up * movementSpeed * Time.deltaTime);
        }
        /*
        if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.PageUp))
        {
            transform.position = transform.position + (Vector3.up * movementSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.PageDown))
        {
            transform.position = transform.position + (-Vector3.up * movementSpeed * Time.deltaTime);
        }*/

        if (looking)
        {
            //float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * freeLookSensitivity;
            //float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * freeLookSensitivity;
            float newRotationX = transform.localEulerAngles.y + _localRotate.x * freeLookSensitivity;
            float newRotationY = transform.localEulerAngles.x - _localRotate.y * freeLookSensitivity;
            transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);
        }

        //float axis = Input.GetAxis("Mouse ScrollWheel");
        //if (axis != 0)
        if(_localZoom > 0)
        {
            var zoomSensitivity = this.zoomSensitivity;
            transform.position = transform.position + transform.forward * _localZoom * zoomSensitivity;
        }

        StartLooking();
    }

    private void OnEnable()
    {
        InputManager.OnFLCamMovement += ApplyTranslation;
        InputManager.OnFLCamRotInput += ApplyRotation;
        InputManager.OnFLCamLifting += ApplyLifting;
    }

    private void OnDisable()
    {
        InputManager.OnFLCamMovement -= ApplyTranslation;
        InputManager.OnFLCamRotInput -= ApplyRotation;
        InputManager.OnFLCamLifting -= ApplyLifting;
        StopLooking();
    }

    /// <summary>
    /// Enable free looking.
    /// </summary>
    public void StartLooking()
    {
        looking = true;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// Disable free looking.
    /// </summary>
    public void StopLooking()
    {
        looking = false;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
