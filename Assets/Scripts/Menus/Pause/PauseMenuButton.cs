using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MenuUtilities;

[ExecuteInEditMode]
public class PauseMenuButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _btnTxt;
    [SerializeField] string _btnLabel;
    [SerializeField] PauseMenuAction _action;

    Button _btn;

    public delegate void ResumeEv();
    public static event ResumeEv OnResume;
    public delegate void StartTutorialEv();
    public static event StartTutorialEv OnStartTutorial;
    public delegate void MainMenuEv();
    public static event MainMenuEv OnMainMenu;
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
        if (_action != PauseMenuAction.None)
        {
            Debug.Log($"{GetType().Name}.cs > Button action is {_action}.");

            switch (_action)
            {
                case PauseMenuAction.Resume:
                    OnResume?.Invoke();

                    break;

                case PauseMenuAction.Tutorial:
                    OnResume?.Invoke();
                    OnStartTutorial?.Invoke();

                    break;

                case PauseMenuAction.Main:
                    OnMainMenu?.Invoke();

                    break;

                case PauseMenuAction.Quit:
                    OnQuit?.Invoke();

                    break;
            }
        }
    }
    
}
