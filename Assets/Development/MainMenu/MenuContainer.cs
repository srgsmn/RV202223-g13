using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;

public class MenuContainer : MonoBehaviour
{
    [SerializeField] private MainMenuScreen screen;

    void Awake()
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
        Debug.Log($"{GetType().Name}.cs > Component screen property is {this.screen} while selected screen is {screen}");

        if (this.screen == screen) {
            Debug.Log($"{GetType().Name}.cs > Enabling component");
            gameObject.SetActive(true);
        }
        else
        {
            Debug.Log($"{GetType().Name}.cs > Disabling component");
            gameObject.SetActive(false);
        }
            
    }
}
