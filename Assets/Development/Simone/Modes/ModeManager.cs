using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Utilities;
using UnityEngine.EventSystems;

public class ModeManager : MonoBehaviour
{
    [SerializeField][ReadOnlyInspector] Mode activeMode;

    [Header("UI elements:")]
    [SerializeField] Image frame;
    [SerializeField] GameObject navM, editM, planM, generic, error, inventory;
    [SerializeField] GameObject firstButton;
    [SerializeField][ReadOnlyInspector] EventSystem eventSystem; 

    private void Awake()
    {
        EventsSubscriber();

        eventSystem = EventSystem.current;
    }

    private void Start()
    {
        NotificationReset();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            InputManager.OnChangeMode += ChangeMode;
        }
        else
        {
            InputManager.OnChangeMode -= ChangeMode;
        }
    }

    private void ChangeMode(Mode mode)
    {
        switch (mode)
        {
            case Mode.Nav:
                inventory.SetActive(false);
                frame.color = new Color32(255, 255, 255, 0);
                navM.SetActive(true);
                Hide(navM);

                break;

            case Mode.Edit:
                inventory.SetActive(false);
                frame.color = new Color32(255, 170, 0, 25);
                editM.SetActive(true);
                Hide(editM);

                break;

            case Mode.Plan:
                ActivateInventory();
                frame.color = new Color32(0, 170, 255, 25);
                planM.SetActive(true);
                Hide(planM);

                break;
        }

        activeMode = mode;
    }

    private void NotificationReset()
    {
        navM.SetActive(false);
        editM.SetActive(false);
        planM.SetActive(false);
        generic.SetActive(false);
        error.SetActive(false);
    }

    IEnumerator Hide(GameObject obj)
    {
        yield return new WaitForSeconds(3);

        obj.SetActive(false);
    }

    private void ActivateInventory()
    {
        inventory.SetActive(true);

        eventSystem.firstSelectedGameObject = firstButton;

        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
