using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;
using UnityEngine.UI;
using TMPro;

[ExecuteInEditMode]
public class MenuButton : MonoBehaviour
{
    [SerializeField] private string label;
    [SerializeField] private TextMeshProUGUI txtArea;
    [SerializeField] private MainMenuScreen linkTo;

    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();

        if (btn == null)
            Debug.LogWarning($"{GetType().Name}.cs > Unlinked Button");

        EventSubscriber();
    }

    private void Start()
    {
        txtArea.text = label;
    }

    private void Update()
    {
        if (!Application.IsPlaying(gameObject))
        {
            txtArea.text = label;
        }
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
            btn.onClick.AddListener(OnClick);
        }
        else
        {
            // Events to unsubscribe
            btn.onClick.RemoveListener(OnClick);
        }
    }

    private void OnClick()
    {
        Debug.Log($"{GetType().Name}.cs > Button {linkTo} clicked, opening screen");

        MainMenuManager.Instance.DisplayScreen(linkTo);
    }
}