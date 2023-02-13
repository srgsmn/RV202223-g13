using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MenuUtilities;

public class PlayButton : MonoBehaviour
{
    [SerializeField] PlayScene _playScene;

    Button _btn;

    public delegate void PlaySceneEv(PlayScene newScene);
    public static event PlaySceneEv AskNewScene;

    public delegate void QuitEv();
    public static event QuitEv AskForQuit;

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

        //MainMenuManager.Instance.QuitApp();
        AskForQuit?.Invoke();
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
        AskNewScene?.Invoke(_playScene);
    }
}
