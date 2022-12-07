using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;

public class MenuContainer : MonoBehaviour
{
    [SerializeField] private MainMenuScreen screen;

    void Start()
    {
        EventSubscriber();
    }

    private void OnDestroy()
    {
        EventSubscriber(false);
    }

    /// <summary>
    /// Function that wraps all subscriptions and unsubscriptions to events
    /// </summary>
    /// <param name="subscribing">If none or true it subscribes, if false it unsubscribes</param>
    private void EventSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            MainMenuManager.OnDisplayScreen += DisplaySelf;
        }
        else
        {
            // Events to unsubscribe
            MainMenuManager.OnDisplayScreen -= DisplaySelf;
        }
    }

    private void DisplaySelf(MainMenuScreen screen)
    {
        if (this.screen == screen)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }
}
