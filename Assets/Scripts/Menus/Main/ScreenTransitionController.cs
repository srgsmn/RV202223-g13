using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using MenuUtilities;

public class ScreenTransitionController : MonoBehaviour
{
    [Header("Panels animators:")]
    [SerializeField] Animator _sidebar;
    [SerializeField] Animator _caption, _details, _alert;
    [Header("First Button:")]
    [SerializeField] GameObject firstButton;

    [SerializeField] private EventSystem eventSystem;

    private void Awake()
    {
        DoChecks();
        /*
        eventSystem = GetComponent<EventSystem>();
        if (eventSystem == null)
        {
            Debug.LogError($"{GetType().Name}.cs > ### EVENT SYSTEM IS NULL ###");
        }
        */
        eventSystem = EventSystem.current;
    }

    private void OnEnable()
    {
        Debug.Log($"{GetType().Name}.cs > First selected is now {((eventSystem.firstSelectedGameObject!=null) ? eventSystem.firstSelectedGameObject : null)}");
        Debug.Log($"{GetType().Name}.cs > CHANGING First Selected in Event System (should be {firstButton})");
        eventSystem.firstSelectedGameObject = firstButton;

        Debug.Log($"{GetType().Name}.cs > First selected is now {eventSystem.firstSelectedGameObject}");

        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void Open()
    {
        Debug.Log($"{GetType().Name}.cs > OPENING {gameObject.name} screen");

        if (_sidebar != null) _sidebar.SetBool(CONSTS.ANIM_FLAG, true);
        if (_caption!=null) _caption.SetBool(CONSTS.ANIM_FLAG, true);
        if (_details != null) _details.SetBool(CONSTS.ANIM_FLAG, true);
        if (_alert != null) _alert.SetBool(CONSTS.ANIM_FLAG, true);
    }

    public void Close()
    {
        Debug.Log($"{GetType().Name}.cs > CLOSING {gameObject.name} screen");

        if (_sidebar != null) _sidebar.SetBool(CONSTS.ANIM_FLAG, false);
        if (_caption != null) _caption.SetBool(CONSTS.ANIM_FLAG, false);
        if (_details != null) _details.SetBool(CONSTS.ANIM_FLAG, false);
        if (_alert != null) _alert.SetBool(CONSTS.ANIM_FLAG, false);
    }

    public void Show(bool flag = true)
    {
        Debug.Log($"{GetType().Name}.cs > CLOSING {gameObject.name} screen");

        gameObject.SetActive(flag);
    }

    private void DoChecks()
    {
        bool allNull = true;

        if (_sidebar != null) allNull = false;
        if (_caption != null) allNull = false;
        if (_details != null) allNull = false;
        if (_alert != null) allNull = false;

        if (allNull)
        {
            Debug.LogError($"{GetType().Name}.cs > There is no panel linked to this component! At least one should be present in order to work properly!");
        }
    }
}
