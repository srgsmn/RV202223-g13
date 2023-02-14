using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] GameObject[] panels;

    [SerializeField] int activeIndex = 0;

    PlayerInputs inputs;

    public delegate void TutorialEndsEv();
    public static event TutorialEndsEv OnTutorialEnds;

    private void Awake()
    {
        inputs = new PlayerInputs();

        EventsSubscriber();
    }

    private void OnEnable()
    {
        inputs.Enable();
    }

    private void Start()
    {
        panels[activeIndex].SetActive(true);
    }

    private void OnDisable()
    {
        inputs.Disable();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            //inputs
            inputs.UI.Back.started += OnBackPressed;
            inputs.UI.Space.started += OnSpacePressed;
        }
        else
        {
            //inputs
            inputs.UI.Back.started -= OnBackPressed;
            inputs.UI.Space.started += OnSpacePressed;
        }
    }

    private void OnBackPressed(InputAction.CallbackContext context)
    {
        if (activeIndex > 0)
        {
            panels[activeIndex--].SetActive(false);
            panels[activeIndex].SetActive(true);
        }
    }

    private void OnSpacePressed(InputAction.CallbackContext context)
    {
        if (activeIndex < panels.Length-1)
        {
            panels[activeIndex++].SetActive(false);
            panels[activeIndex].SetActive(true);
        }
        else if (activeIndex == panels.Length - 1)
        {
            panels[activeIndex].SetActive(false);

            OnTutorialEnds?.Invoke();
        }
    }
}
