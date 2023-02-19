using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // SINGLETON

    [Header("Scene:")]
    [SerializeField][ReadOnlyInspector] SceneType activeScene;
    [SerializeField][ReadOnlyInspector] public int sceneIndex = 0;
    [SerializeField][ReadOnlyInspector] SceneState state;
    [SerializeField][ReadOnlyInspector] string sessionID;

    [Header("Loading:")]
    [SerializeField][ReadOnlyInspector] bool isLoading = false;

    [Header("Tutorial:")]
    [SerializeField][ReadOnlyInspector] GameObject tutorialPrefab;
    [SerializeField][ReadOnlyInspector] private bool tutorialOn = false;
    private GameObject tutorialInstance;

    [Header("Pause:")]
    [SerializeField][ReadOnlyInspector] GameObject pauseMenuPrefab;
    [SerializeField][ReadOnlyInspector] bool isPaused = false;
    private GameObject pauseMenuInstance;

    [Header("Mode HUD:")]
    [SerializeField][ReadOnlyInspector] GameObject modeHUDPrefab;
    private GameObject modeHUDInstance;

    [Header("Mode HUD:")]
    [SerializeField][ReadOnlyInspector] GameObject finalePrefab;
    private GameObject finaleInstance;

    /*
    [Header("Event System:")]
    [SerializeField][ReadOnlyInspector] EventSystem eventSystem;
    [SerializeField][ReadOnlyInspector] GameObject selectedGO;
    */

    public delegate void PauseEv(bool isPaused = true);
    public static event PauseEv OnPause;
    public delegate void StateChangedEv(SceneType type, SceneState state);
    public static event StateChangedEv OnSceneUpdate;

    private void Awake()
    {
        // SINGLETON
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(Instance);
        }
        else
        {
            Destroy(gameObject);
        }

        tutorialPrefab = Resources.Load(PREFABS.TUTORIAL) as GameObject;
        pauseMenuPrefab = Resources.Load(PREFABS.PAUSE) as GameObject;
        modeHUDPrefab = Resources.Load(PREFABS.HUD) as GameObject;
        finalePrefab = Resources.Load(PREFABS.FINALE) as GameObject;

        if (tutorialPrefab == null)
            Debug.LogError($"{GetType().Name}.cs > Tutorial prefab is MISSING");

        //eventSystem = EventSystem.current;

        EventsSubscriber();
    }

    /*
    private void Update()
    {
        //selectedGO = eventSystem.currentSelectedGameObject;
    }
    */

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            PlayButton.AskNewScene += DisplayScene;
            PlayButton.AskForQuit += QuitApp;

            PopUpButton.OnQuit += QuitApp;

            SBButton.OnQuit += QuitApp;

            SceneManager.sceneLoaded += OnSceneLoaded;

            TutorialManager.OnTutorialEnds += HideTutorial;

            // From pause menu
            PauseMenuButton.OnResume += OnResumeInput;
            PauseMenuButton.OnStartTutorial += ShowTutorial;
            PauseMenuButton.OnMainMenu += ShowMainMenu;
            PauseMenuButton.OnQuit += QuitApp;

            // From Input Manager
            InputManager.OnPause += OnPauseInput;
            InputManager.OnResume += OnResumeInput;
        }
        else
        {
            PlayButton.AskNewScene -= DisplayScene;
            PlayButton.AskForQuit -= QuitApp;

            PopUpButton.OnQuit -= QuitApp;

            SBButton.OnQuit -= QuitApp;

            SceneManager.sceneLoaded -= OnSceneLoaded;

            TutorialManager.OnTutorialEnds -= HideTutorial;

            // From pause menu
            PauseMenuButton.OnResume -= OnResumeInput;
            PauseMenuButton.OnStartTutorial -= ShowTutorial;
            PauseMenuButton.OnMainMenu -= ShowMainMenu;
            PauseMenuButton.OnQuit -= QuitApp;

            // From Input Manager
            InputManager.OnPause -= OnPauseInput;
            InputManager.OnResume -= OnResumeInput;
        }
    }

    /// <summary>
    /// Change scene and display demo/uploaded environment or startup menu
    /// </summary>
    /// <param name="newScene"></param>
    private void DisplayScene(SceneType newScene)
    {
        switch (newScene)
        {
            case SceneType.MainMenu:
                Debug.Log($"{GetType().Name}.cs > Loading {newScene} scene");

                SetPause(false);

                SceneManager.LoadScene(1);

                break;

            case SceneType.Demo1:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD DEMO 1 SCENE");

                SceneManager.LoadScene(2);

                break;

            case SceneType.Demo2:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD DEMO 2 SCENE");

                SceneManager.LoadScene(3);

                break;

            case SceneType.Browse:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD BROWSE SCENE");

                //SceneManager.LoadScene(4);

                break;
        }

        activeScene = newScene;

        OnSceneUpdate(activeScene, SceneState.None);
    }

    /// <summary>
    /// Operations to do after a new scene is loaded
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneIndex = scene.buildIndex;

        // Check index for tutorial
        if (sceneIndex > 1)
        {
            ShowHUD();
            ShowTutorial();

            SetSessionID();
        }
    }

    private void ShowHUD()
    {
        modeHUDInstance = Instantiate(modeHUDPrefab);
    }

    private void ShowTutorial()
    {
        Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD THE TUTORIAL");

        tutorialInstance = Instantiate(tutorialPrefab);

        tutorialOn = true;

        state = SceneState.Tutorial;

        Time.timeScale = 0;

        OnSceneUpdate?.Invoke(activeScene, state);
    }

    private void HideTutorial()
    {
        Destroy(tutorialInstance);

        tutorialOn = false;

        state = SceneState.Playing;

        Time.timeScale = 1;

        OnSceneUpdate?.Invoke(activeScene, state);
    }

    private void ShowEndScreen()
    {
        modeHUDInstance.SetActive(false);
        finaleInstance = Instantiate(finalePrefab);
    }

    public void ShowMainMenu()
    {
        state = SceneState.None;

        DisplayScene(SceneType.MainMenu);

        OnSceneUpdate?.Invoke(activeScene, state);
    }

    public void EndsGame()
    {
        if (state == SceneState.Playing)
        {
            state = SceneState.Endgame;

            ShowEndScreen();
        }
    }

    public void ReloadScene()
    {
        DisplayScene(activeScene);
    }

    private void OnPauseInput()
    {
        SetPause(true);
    }

    private void OnResumeInput()
    {
        SetPause(false);
    }

    private void SetPause(bool pausing = true)
    {
        // TODO disable charachter input, link to input manager
        isPaused = pausing;

        OnPause?.Invoke(isPaused);

        if (isPaused)
        {
            Debug.Log($"{GetType().Name}.cs > FREEZING the app and showing pause menu");

            Time.timeScale = 0;

            state = SceneState.Paused;

            pauseMenuInstance = Instantiate(pauseMenuPrefab);
        }
        else
        {
            Debug.Log($"{GetType().Name}.cs > UNFREEZING the app and destroying pause menu");

            Destroy(pauseMenuInstance);

            Time.timeScale = 1;

            state = SceneState.Playing;
        }

        OnSceneUpdate?.Invoke(activeScene, state);
    }

    private void SetSessionID()
    {
        sessionID = DateTime.Now.ToString("yyyy-MM-dd-HH-mmss").Replace("-", "");
    }

    public string GetSessionID()
    {
        return sessionID;
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
}
