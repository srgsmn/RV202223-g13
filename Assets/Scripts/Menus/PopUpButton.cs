using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MenuUtilities;

public class PopUpButton : MonoBehaviour
{
    [SerializeField] PopUpOpt _option;

    Button _btn;

    public delegate void DeleteEv();
    public static DeleteEv DeleteGO;

    public delegate void MoveBtnEv();
    public static MoveBtnEv MoveGO;

    private void Awake()
    {
        _btn = GetComponent<Button>();

        EventsSubscriber();

        DoChecks();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            _btn.onClick.AddListener(OnClick);
        }
        else
        {
            // Events to unsubscribe
            _btn.onClick.RemoveListener(OnClick);
        }
    }

    public void QuitApp()
    {
        Debug.Log($"{GetType().Name}.cs > QUITTING the app...");

        MainMenuManager.Instance.QuitApp();
    }

    private void DoChecks()
    {
        if (_btn == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Button component not found");
        }
    }

    public virtual void OnClick()
    {
        switch (_option)
        {
            case PopUpOpt.Move:
                Debug.LogWarning("### TODO ### Here it goes the implementation of the movement");

                MoveGO?.Invoke();

                break;

            case PopUpOpt.Delete:
                Debug.Log($"{GetType().Name}.cs > Deleting the gameobject");

                DeleteGO?.Invoke();

                break;
        }
    }
}
