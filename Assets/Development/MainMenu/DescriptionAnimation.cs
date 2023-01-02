using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;


public class DescriptionAnimation : MonoBehaviour
{
    [SerializeField] private bool isHidden;
    [SerializeField] private Environments environment;

    private void Awake()
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
            StartMenuButton.OnEnvSelect += DisplaySelf;
        }
        else
        {
            // Events to unsubscribe
            StartMenuButton.OnEnvSelect += DisplaySelf;
        }
    }

    private void DisplaySelf(Environments target)
    {
        if (environment == target)
        {
            //TODO
        }
    }
}
