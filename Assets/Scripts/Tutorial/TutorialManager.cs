using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    // SINGLETON
    public static TutorialManager Instance;

    [SerializeField] GameObject[] panels;

    [SerializeField] int activeIndex = 0;

    public delegate void TutorialEndsEv();
    public static event TutorialEndsEv OnTutorialEnds;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        EventsSubscriber();
    }

    private void Start()
    {
        panels[activeIndex].SetActive(true);
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
            InputManager.OnTutorialPageUpdate += PageUpdate;
        }
        else
        {
            //inputs
            InputManager.OnTutorialPageUpdate -= PageUpdate;
        }
    }

    private void PageUpdate(bool forward = true)
    {
        if (forward)
        {
            if (activeIndex < panels.Length - 1)
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
        else
        {
            if (activeIndex > 0)
            {
                panels[activeIndex--].SetActive(false);
                panels[activeIndex].SetActive(true);
            }
        }
    }

    /*
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
    */
}
