using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using MenuUtilities;
using UnityEditor;

/// <summary>
/// Manager script for Main Menu Scene. It contains the FSM that controls the screens.
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    // SINGLETON
    public static MainMenuManager Instance;

    public static string testString = "test";

    private PlayerInputs inputs;

    //FIELDS
    [SerializeField] MainMenuScreen _activeScreen;
    [SerializeField] MainMenuItem[] _screenList;

    Dictionary<MainMenuScreen, int> _screenIndexes;

    Stack<MainMenuScreen> _screenHistory = new Stack<MainMenuScreen>();


    // EVENTS
    public delegate void DisplayPanelEv(MainMenuScreen screen);
    public static DisplayPanelEv OnDisplayScreen;

    // FUNCTIONS
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        inputs = new PlayerInputs();

        EventsSubscriber();

        BuildDictionary();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void Start()
    {
        HideAll();
        DisplayScreen(MainMenuScreen.Main);
    }

    private void HideAll()
    {
        Debug.Log($"{GetType().Name}.cs > HIDING all screens");

        foreach (MainMenuItem item in _screenList)
        {
            Debug.Log($"{GetType().Name}.cs > Hiding {item.type} screen");

            item.gameObject.GetComponent<ScreenTransitionController>().Close();
            item.gameObject.GetComponent<ScreenTransitionController>().Show(false);
        }
    }

    public void DisplayScreen(MainMenuScreen screen)
    {
        Debug.Log($"{GetType().Name}.cs > Changing screen from {_activeScreen} to {screen}");

        MainMenuScreen targetScreen = MainMenuScreen.None;

        if (screen != MainMenuScreen.None)
        {
            Debug.Log($"{GetType().Name}.cs > Screen to display is not None but {screen}");

            if (screen == MainMenuScreen.Prev)
            {
                Debug.Log($"{GetType().Name}.cs > Previous screen requested. Screen history is {String.Join(",", _screenHistory)}: popping last one...");

                targetScreen = _screenHistory.Pop();

                Debug.Log($"{GetType().Name}.cs > Popped screen is {targetScreen}");
            }
            else
            {
                Debug.Log($"{GetType().Name}.cs > {screen} screen requested. Selecting it as the target screen...");

                targetScreen = screen;

                if (_activeScreen != MainMenuScreen.None)
                {
                    Debug.Log($"{GetType().Name}.cs > Pushing {_activeScreen} in the screen history");
                    _screenHistory.Push(_activeScreen);
                }

                Debug.Log($"{GetType().Name}.cs > Target screen is now {targetScreen} while screen history is {String.Join(",", _screenHistory)}");
            }

            if (_activeScreen != MainMenuScreen.None) {
                Debug.Log($"{GetType().Name}.cs > Active screen is {_activeScreen}, moving it out");

                //_screenList[_screenIndexes[_activeScreen]].gameObject.SetActive(false); //change to trigger animations
                _screenList[_screenIndexes[_activeScreen]].gameObject.GetComponent<ScreenTransitionController>().Close();

                StartCoroutine(Unshow(_activeScreen));
                //_screenList[_screenIndexes[_activeScreen]].gameObject.GetComponent<ScreenTransitionController>().Show(false);
            }
            else
            {
                Debug.Log($"{GetType().Name}.cs > Active screen is None ({_activeScreen}), so nothing to move out");
            }

            _activeScreen = targetScreen;
            Debug.Log($"{GetType().Name}.cs > Active screen now set to {_activeScreen}: moving it in");

            //_screenList[_screenIndexes[_activeScreen]].gameObject.SetActive(true);
            _screenList[_screenIndexes[_activeScreen]].gameObject.GetComponent<ScreenTransitionController>().Show();
            _screenList[_screenIndexes[_activeScreen]].gameObject.GetComponent<ScreenTransitionController>().Open();
        }
    }

    IEnumerator Unshow(MainMenuScreen screen)
    {
        Debug.Log($"{GetType().Name}.cs > Coroutine started (to let animation be done)");

        yield return new WaitForSeconds(1);

        Debug.Log($"{GetType().Name}.cs > Deactivating the panel)");
        _screenList[_screenIndexes[screen]].gameObject.GetComponent<ScreenTransitionController>().Show(false);
    }

    /// <summary>
    /// Change scene and display demo/uploaded environment or startup menu
    /// </summary>
    /// <param name="newScene"></param>
    private void DisplayScene(PlayScene newScene)
    {
        switch (newScene)
        {
            case PlayScene.MainMenu:
                Debug.Log($"{GetType().Name}.cs > Loading {newScene} scene");

                SceneManager.LoadScene(1);

                break;

            case PlayScene.Demo1:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD DEMO 1 SCENE");
                //SceneManager.LoadScene(2);

                break;

            case PlayScene.Demo2:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD DEMO 2 SCENE");
                //SceneManager.LoadScene(3);

                break;

            case PlayScene.Browse:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD BROWSE SCENE");
                //SceneManager.LoadScene(4);

                break;
        }
    }

    /// <summary>
    /// Quits the app. Private method accessible through events.
    /// </summary>
    public void QuitApp()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            PlayButton.AskNewScene += DisplayScene;

            //inputs
            inputs.UI.Back.started += OnBackPressed;
            inputs.UI.Back.performed += OnBackPressed;
            inputs.UI.Back.canceled += OnBackPressed;
        }
        else
        {
            PlayButton.AskNewScene -= DisplayScene;

            //inputs
            inputs.UI.Back.started -= OnBackPressed;
            inputs.UI.Back.performed -= OnBackPressed;
            inputs.UI.Back.canceled -= OnBackPressed;
        }
    }

    private void OnBackPressed(InputAction.CallbackContext context)
    {
        Debug.Log($"{GetType().Name}.cs > Backspace pressed");

        if (context.ReadValueAsButton())
        {
            DisplayScreen(MainMenuScreen.Prev);
        }

        DisplayScreen(MainMenuScreen.Prev);
    }

    private void BuildDictionary()
    {
        if (_screenList != null)
        {
            _screenIndexes = new Dictionary<MainMenuScreen, int>();

            int i = 0;
            foreach(MainMenuItem screen in _screenList)
            {
                _screenIndexes.Add(screen.type, i++);
            }
        }
        else
        {
            Debug.LogWarning($"{GetType().Name}.cs > No menu set in the inspector");
        }
    }
}
