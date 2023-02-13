using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance; // SINGLETON

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
        }
        else
        {
            PlayButton.AskNewScene -= DisplayScene;
            PlayButton.AskForQuit -= QuitApp;

            PopUpButton.OnQuit -= QuitApp;

            SBButton.OnQuit -= QuitApp;
        }
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
}
