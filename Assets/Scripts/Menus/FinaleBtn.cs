using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

[ExecuteInEditMode]
public class FinaleBtn : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _btnTxt;
    [SerializeField] string _btnLabel;
    [Header("Save info:")]
    [SerializeField] FileManager fileManager;
    [SerializeField] FileFormat fileFormat;

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
        }

        if (!_btn.interactable)
        {
            _btnTxt.color = Color.white;
        }
    }

    public void ShowMenu()
    {
        Debug.Log($"{GetType().Name}.cs > REDIRECTING to main menu...");

        GameManager.Instance.ShowMainMenu();
    }

    public void RestartScene()
    {
        Debug.Log($"{GetType().Name}.cs > RESTARTING the scene...");

        GameManager.Instance.ReloadScene();
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
        Debug.Log($"{GetType().Name}.cs > BUTTON CLICKED");

        //fileManager.SaveReport();

        /*
        if (_linkTo != MainMenuScreen.None)
        {
            Debug.Log($"{GetType().Name}.cs > Button to {_linkTo} screen clicked. Asking Main Menu Manager to display {_linkTo} screen");
            MainMenuManager.Instance.DisplayScreen(_linkTo);
        }
        */
    }
}
