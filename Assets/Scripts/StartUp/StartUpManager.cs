using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class StartUpManager : MonoBehaviour
{
    private PlayerInputs inputs;

    private void Awake()
    {
        inputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        EventsSubscriber();

        inputs.Enable();
    }

    private void OnDisable()
    {
        inputs.Disable();

        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            //inputs
            //inputs.UI.Space.started += OnSpacePressed;
            inputs.UI.Space.performed += OnSpacePressed;
            //inputs.UI.Space.canceled += OnSpacePressed;
        }
        else
        {
            //inputs
            //inputs.UI.Space.started -= OnSpacePressed;
            inputs.UI.Space.performed -= OnSpacePressed;
            //inputs.UI.Space.canceled -= OnSpacePressed;
        }
    }

    private void OnSpacePressed(InputAction.CallbackContext context)
    {
        if(context.ReadValueAsButton())
        {
            Debug.Log($"{GetType().Name}.cs > Space key pressed: redirecting to main menu");

            SceneManager.LoadScene(1);
        }
    }
}
