using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // SINGLETON

    [Header("Scene:")]
    [SerializeField][ReadOnlyInspector] SceneType activeScene;
    [SerializeField][ReadOnlyInspector] public int sceneIndex = 0;
    [SerializeField][ReadOnlyInspector] SceneState state;
    [SerializeField][ReadOnlyInspector] string sessionTimestamp;
    [SerializeField][ReadOnlyInspector] string sessionID;

    [Header("Loading:")]
    [SerializeField][ReadOnlyInspector] GameObject loadingPanelPrefab;
    [SerializeField][ReadOnlyInspector] GameObject loadingPanelInstance;
    [SerializeField][ReadOnlyInspector] Slider loadingSlider;
    [SerializeField][ReadOnlyInspector] bool isLoading = false;

    [Header("Tutorial:")]
    [SerializeField][ReadOnlyInspector] GameObject tutorialPrefab;
    [SerializeField][ReadOnlyInspector] private bool tutorialOn = false;
    private GameObject _tutorialInstance;

    [Header("Pause:")]
    [SerializeField][ReadOnlyInspector] GameObject pauseMenuPrefab;
    [SerializeField][ReadOnlyInspector] bool isPaused = false;
    private GameObject _pauseMenuInstance;

    [Header("Mode HUD:")]
    [SerializeField][ReadOnlyInspector] GameObject modeHUDPrefab;
    private GameObject _HUDInstance;

    [Header("Finale:")]
    [SerializeField][ReadOnlyInspector] GameObject finalePrefab;
    [SerializeField][ReadOnlyInspector] bool isFinale = false;
    private GameObject finaleInstance;

    [Header("Event System:")]
    [SerializeField][ReadOnlyInspector] EventSystem eventSystem;
    [SerializeField][ReadOnlyInspector] GameObject selectedGO;

    public delegate void PauseEv(bool isPaused = true);
    public static event PauseEv OnPause;
    public delegate void StateChangedEv(SceneType type, SceneState state);
    public static event StateChangedEv OnSceneUpdate;
    public delegate void ReportEv(ReportType type, string message);
    public static event ReportEv OnReport;
    //public delegate void NewSceneIdEv(int i);
    //public static event NewSceneIdEv OnNewSceneIndex;

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
        loadingPanelPrefab = Resources.Load(PREFABS.LOADING) as GameObject;

        if (tutorialPrefab == null)
            Debug.LogError($"{GetType().Name}.cs > Tutorial prefab is MISSING");

        eventSystem = EventSystem.current;

        EventsSubscriber();
    }

    private void Update()
    {
        selectedGO = eventSystem.currentSelectedGameObject;
    }

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

            OnSceneUpdate += CheckState;
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

            OnSceneUpdate -= CheckState;
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

                StartCoroutine(LoadSceneAsync(2));

                break;

            case SceneType.Demo2:

                StartCoroutine(LoadSceneAsync(3));

                break;

            case SceneType.Browse:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD BROWSE SCENE");

                StartCoroutine(LoadSceneAsync(4));

                break;
        }

        activeScene = newScene;

        OnSceneUpdate?.Invoke(activeScene, SceneState.None);
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        loadingPanelInstance = Instantiate(loadingPanelPrefab);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        loadingPanelInstance.SetActive(true);

        loadingSlider = loadingPanelInstance.GetComponentInChildren<Slider>();

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.9f);

            Debug.Log($"{GetType().Name}.cs > Loading progress value is {progressValue}");

            loadingSlider.value = progressValue;

            yield return null;
        }
    }

    /// <summary>
    /// Operations to do after a new scene is loaded
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneIndex = scene.buildIndex;

        //OnNewSceneIndex?.Invoke(sceneIndex);

        // Check index for tutorial
        if (sceneIndex > 1)
        {
            ShowHUD();
            ShowTutorial();

            SetSessionData();
        }
    }

    private void ShowHUD()
    {
        _HUDInstance = Instantiate(modeHUDPrefab);
    }

    private void ShowTutorial()
    {
        _tutorialInstance = Instantiate(tutorialPrefab);

        tutorialOn = true;

        state = SceneState.Tutorial;

        Time.timeScale = 0;

        OnSceneUpdate?.Invoke(activeScene, state);
    }

    private void HideTutorial()
    {
        Destroy(_tutorialInstance);

        tutorialOn = false;

        state = SceneState.Playing;

        Time.timeScale = 1;

        OnSceneUpdate?.Invoke(activeScene, state);
    }

    private void ShowFinalScreen()
    {
        _HUDInstance.SetActive(false);

        Debug.Log($"{GetType().Name}.cs > Freezing background");

        Time.timeScale = 0;

        Debug.Log($"{GetType().Name}.cs > {Time.timeScale} Destroying HUD and pause");

        Destroy(_HUDInstance);
        //Destroy(_pauseMenuInstance);

        finaleInstance = Instantiate(finalePrefab);
    }

    public void ShowMainMenu()
    {
        state = SceneState.None;

        DisplayScene(SceneType.MainMenu);

        OnSceneUpdate?.Invoke(activeScene, state);
    }

    public void RestartScene()
    {
        DisplayScene(activeScene);
    }

    public void EndsGame()
    {
        if (state == SceneState.Playing)
        {
            state = SceneState.Endgame;
            isFinale = true;

            ShowFinalScreen();
        }
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

        if (!isFinale)
        {
            if (isPaused)
            {
                Debug.Log($"{GetType().Name}.cs > FREEZING the app and showing pause menu");

                Time.timeScale = 0;

                state = SceneState.Paused;

                _pauseMenuInstance = Instantiate(pauseMenuPrefab);
            }
            else
            {
                Debug.Log($"{GetType().Name}.cs > UNFREEZING the app and destroying pause menu");

                Destroy(_pauseMenuInstance);

                Time.timeScale = 1;

                state = SceneState.Playing;
            }

            OnSceneUpdate?.Invoke(activeScene, state);
        }
    }

    private void SetSessionData()
    {
        sessionTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        sessionID = DateTime.Now.ToString("yyyy-MM-dd HH-mmss").Replace("-", "").Replace(":","").Replace(" ", "");
    }

    public string GetSessionTimestamp()
    {
        return sessionTimestamp;
    }

    public string GetSessionID()
    {
        return sessionID;
    }

    public void MakeReport(ReportType type, string message)
    {
        OnReport?.Invoke(type, message);
    }

    private void CheckState(SceneType type, SceneState state)
    {
        if(type==SceneType.Demo1 || type == SceneType.Demo2 || type == SceneType.Browse)
        {
            switch (state)
            {
                case SceneState.Tutorial:
                case SceneState.Playing:
                    //Cursor.visible = false;
                    //Cursor.lockState = CursorLockMode.Locked;

                    break;

                case SceneState.Paused:
                case SceneState.Endgame:
                    //Cursor.visible = true;
                    //Cursor.lockState = CursorLockMode.None;

                    break;
            }
        }
        else
        {
            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
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
}
