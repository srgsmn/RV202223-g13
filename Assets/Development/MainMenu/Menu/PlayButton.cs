using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MenuUtilities;

public class PlayButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _btnTxt;
    [SerializeField] PlayScene _playScene;

    Button _btn;

    public delegate void PlaySceneEv(PlayScene newScene);
    public static PlaySceneEv AskNewScene;

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

        if (_btnTxt == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Button Text component not set in the inspector");
        }
    }

    public virtual void OnClick()
    {
        AskNewScene?.Invoke(_playScene);
    }
}
