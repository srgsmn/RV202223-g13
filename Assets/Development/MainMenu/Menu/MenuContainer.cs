using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;

public class MenuContainer : MonoBehaviour
{
    [SerializeField] private MainMenuScreen screen;
    [SerializeField] private GameObject Sidebar;
    [SerializeField] private GameObject Description;
    [SerializeField] private bool doTransition = true;

    private Animator sidebarAnim, descAnim;

    void Awake()
    {
        EventSubscriber();

        sidebarAnim = Sidebar.GetComponent<Animator>();

        if (sidebarAnim == null)
            Debug.Log($"{GetType().Name}.cs > Sidebar animator not found");

        descAnim = Description.GetComponent<Animator>();

        if (descAnim == null)
            Debug.Log($"{GetType().Name}.cs > Description animator not found");
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

            if (doTransition)
            {
                StartCoroutine(ChangeSidebarState());
            }
            
        }
        else
        {
            Debug.Log($"{GetType().Name}.cs > Disabling component");

            if(gameObject.activeSelf && doTransition)
                StartCoroutine(ChangeSidebarState());

            gameObject.SetActive(false);
        }
            
    }

    IEnumerator ChangeSidebarState()
    {
        Debug.Log($"{GetType().Name}.cs > Changing sidebar state from {(sidebarAnim.GetBool("open") ? "open":"close")} to {(!sidebarAnim.GetBool("open") ? "open" : "close")}");

        if (sidebarAnim != null)
        {
            sidebarAnim.SetBool("open", !sidebarAnim.GetBool("open"));
        }

        if(descAnim != null)
        {
            descAnim.SetBool("open", !descAnim.GetBool("open"));
        }

        Debug.Log($"{GetType().Name}.cs > Before yield ({Time.time})");

        yield return new WaitForSeconds(1);

        Debug.Log($"{GetType().Name}.cs > After yield ({Time.time}, should be +1s)");
    }
}
