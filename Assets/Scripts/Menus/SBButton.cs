using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MenuUtilities;

[ExecuteInEditMode]
public class SBButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _btnTxt;
    [SerializeField] string _btnLabel;
    [SerializeField] MainMenuScreen _linkTo;

    Button _btn;

    public delegate void QuitEv();
    public static event QuitEv OnQuit;

    private void Awake()
    {
        _btn = GetComponent<Button>();

        EventsSubscriber();

        DoChecks();
    }

    void Start()
    {
        ButtonFiller();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            ButtonFiller();
        }
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

    private void ButtonFiller()
    {
        if (_btnLabel != null && _btnLabel != "")
        {
            _btnTxt.text = _btnLabel;
            gameObject.name = _btnLabel + "_btn";

            switch (_btnLabel)
            {
                case CONSTS.BACK_OPT:
                    _linkTo = MainMenuScreen.Prev;

                    break;

                case CONSTS.MAIN_OPT:

                    _linkTo = MainMenuScreen.Main;
                    break;

                case CONSTS.START_OPT:

                    _linkTo = MainMenuScreen.Start;
                    break;

                case CONSTS.REPORTS_OPT:

                    _linkTo = MainMenuScreen.Reports;
                    break;

                case CONSTS.SETTINGS_OPT:

                    _linkTo = MainMenuScreen.Settings;
                    break;

                case CONSTS.ABOUT_OPT:

                    _linkTo = MainMenuScreen.About;
                    break;

                case CONSTS.QUIT_OPT:

                    _linkTo = MainMenuScreen.Quit;
                    break;
            }
        }

        if (!_btn.interactable)
        {
            _btnTxt.color = Color.white;
        }
    }

    public void QuitApp()
    {
        Debug.Log($"{GetType().Name}.cs > QUITTING the app...");

        OnQuit?.Invoke();
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
        if(_linkTo != MainMenuScreen.None)
        {
            Debug.Log($"{GetType().Name}.cs > Button to {_linkTo} screen clicked. Asking Main Menu Manager to display {_linkTo} screen");
            MainMenuManager.Instance.DisplayScreen(_linkTo);
        }
    }
}
