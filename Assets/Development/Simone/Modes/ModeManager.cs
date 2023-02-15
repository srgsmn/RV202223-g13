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
        ChangeMode(Mode.Nav);
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
                Debug.Log($"{GetType().Name}.cs > CHANGING MODE to navigation");

                inventory.SetActive(false);
                frame.color = new Color32(255, 255, 255, 0);
                ForceHide(planM);
                ForceHide(navM);
                navM.SetActive(true);
                navM.transform.SetAsLastSibling();
                StartCoroutine(Hide(navM));

                break;

            case Mode.Edit:
                Debug.Log($"{GetType().Name}.cs > CHANGING MODE to edit");

                inventory.SetActive(false);
                frame.color = new Color32(255, 170, 0, 25);
                ForceHide(planM);
                ForceHide(navM);
                editM.SetActive(true);
                editM.transform.SetAsLastSibling();
                StartCoroutine(Hide(editM));

                break;

            case Mode.Plan:
                Debug.Log($"{GetType().Name}.cs > CHANGING MODE to planning");

                ActivateInventory();
                frame.color = new Color32(0, 170, 255, 25);
                ForceHide(editM);
                ForceHide(navM);
                planM.SetActive(true);
                planM.transform.SetAsLastSibling();
                StartCoroutine(Hide(planM));

                break;
        }

        activeMode = mode;
    }

    private void NotificationReset()
    {
        Debug.Log($"{GetType().Name}.cs > RESET notifications");

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

    private void ForceHide(GameObject obj)
    {
        obj.SetActive(false);
    }

    private void ActivateInventory()
    {
        Debug.Log($"{GetType().Name}.cs > ACTIVATING inventory");

        inventory.SetActive(true);

        eventSystem.firstSelectedGameObject = firstButton;

        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
}
