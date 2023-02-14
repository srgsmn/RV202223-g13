using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using Utilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // SINGLETON

    [Header("Scene:")]
    [SerializeField][ReadOnlyInspector] SceneType activeScene;
    [SerializeField][ReadOnlyInspector] int sceneIndex = 0;
    [SerializeField][ReadOnlyInspector] SceneState state;

    [Header("Loading:")]
    [SerializeField][ReadOnlyInspector] bool isLoading = false;

    [Header("Tutorial:")]
    [SerializeField][ReadOnlyInspector] GameObject tutorialPrefab;
    [SerializeField][ReadOnlyInspector] private bool tutorialOn = false;
    private GameObject tutorialInstance;

    [Header("Pause:")]
    [SerializeField][ReadOnlyInspector] bool isPaused = false;

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

        if(tutorialPrefab == null)
            Debug.LogError($"{GetType().Name}.cs > Tutorial prefab is MISSING");

        EventsSubscriber();
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
        }
        else
        {
            PlayButton.AskNewScene -= DisplayScene;
            PlayButton.AskForQuit -= QuitApp;

            PopUpButton.OnQuit -= QuitApp;

            SBButton.OnQuit -= QuitApp;

            SceneManager.sceneLoaded -= OnSceneLoaded;

            TutorialManager.OnTutorialEnds -= HideTutorial;
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

                SceneManager.LoadScene(1);

                break;

            case SceneType.Demo1:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD DEMO 1 SCENE");

                SceneManager.LoadScene(2);

                break;

            case SceneType.Demo2:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD DEMO 2 SCENE");

                //SceneManager.LoadScene(3);

                break;

            case SceneType.Browse:
                Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD BROWSE SCENE");

                //SceneManager.LoadScene(4);

                break;
        }

        activeScene = newScene;
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
            ShowTutorial();
        }
    }

    private void ShowTutorial()
    {
        Debug.LogWarning("### TODO ### HERE IT SHOULD LOAD THE TUTORIAL");

        tutorialInstance = Instantiate(tutorialPrefab);

        tutorialOn = true;
    }

    private void HideTutorial()
    {
        Destroy(tutorialInstance);

        tutorialOn = false;
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
