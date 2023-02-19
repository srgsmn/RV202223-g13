using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuEventLinker : MonoBehaviour
{
    [SerializeField][ReadOnlyInspector] EventSystem eventSystem;
    [SerializeField][ReadOnlyInspector] Button firstButton, lastHovered;

    private void Awake()
    {
        eventSystem = EventSystem.current;

        EventsSubscriber();
    }

    private void Start()
    {
        FindFirstButton();
    }

    private void OnEnable()
    {
        SetAsFirstButton(firstButton.gameObject);
        lastHovered = firstButton;
    }

    private void Update()
    {
        if (eventSystem.currentSelectedGameObject == null)
        {
            SetAsFirstButton(lastHovered.gameObject);
        }
        else
        {
            if(lastHovered != eventSystem.currentSelectedGameObject.GetComponent<Button>())
                lastHovered = eventSystem.currentSelectedGameObject.GetComponent<Button>();
        }
    }

    private void OnDisable()
    {
        SetAsFirstButton(null);
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void FindFirstButton()
    {
        firstButton = this.transform.GetComponentInChildren<Button>();
    }

    private void SetAsFirstButton(GameObject obj)
    {
        eventSystem.firstSelectedGameObject = obj;
        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            //InputManager.OnArrowPressed += OnButtonPressed;
        }
        else
        {
            //InputManager.OnArrowPressed -= OnButtonPressed;
        }
    }

    private void OnButtonPressed()
    {
        if (gameObject.activeSelf)
        {
            SetAsFirstButton(firstButton.gameObject);
        }
    }
}
