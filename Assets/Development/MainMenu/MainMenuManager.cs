using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance;

    [SerializeField] private MainMenuScreen activeScreen;
    [SerializeField] private Stack<MainMenuScreen> previousScreens = new Stack<MainMenuScreen>();

    // EVENTS
    public delegate void DisplayScreeenEv(MainMenuScreen screen);
    public static DisplayScreeenEv OnDisplayScreen;

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
    }

    private void Start()
    {
        activeScreen = MainMenuScreen.Main;
        OnDisplayScreen?.Invoke(MainMenuScreen.Main);
    }

    public void DisplayScreen(MainMenuScreen screen)
    {
        Debug.Log($"{GetType().Name}.cs > Changing screen from {activeScreen} to {screen}");

        if (screen == MainMenuScreen.Back)
        {
            activeScreen = previousScreens.Pop();

            OnDisplayScreen?.Invoke(activeScreen);
        }
        else
        {
            OnDisplayScreen?.Invoke(screen);

            previousScreens.Push(activeScreen);
            activeScreen = screen;
        }
    }
}
