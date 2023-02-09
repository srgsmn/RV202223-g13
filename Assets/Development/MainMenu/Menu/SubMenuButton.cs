using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class SubMenuButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _btnTxt;
    [SerializeField] string _btnLabel;
    [SerializeField] SubMenuDetail targetDetail;

    Button _btn;

    // EVENTS
    public delegate void DisplayDetailsEv(SubMenuDetail target);
    public static DisplayDetailsEv OnDisplayDetails;


    private void Awake()
    {
        _btn = GetComponent<Button>();

        EventsSubscriber();

        DoChecks();
    }

    void Start()
    {
        ButtonFiller();
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    void Update()
    {
        if (!Application.isPlaying)
        {
            ButtonFiller();
        }
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            _btn.onClick.AddListener(OnClick);
        }
        else
        {
            // Events to unsubscribe
            _btn.onClick.RemoveListener(OnClick);
        }
    }

    private void OnClick()
    {
        if (targetDetail == SubMenuDetail.Back)
        {
            OnDisplayDetails?.Invoke(targetDetail);

            MainMenuManager.Instance.DisplayScreen(MainMenuScreen.Prev);
        }
        else
        {
            OnDisplayDetails?.Invoke(targetDetail);
        }
    }

    private void DoChecks()
    {
        if (_btn == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Button component not found");
        }

        if (_btnTxt == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Button Text component not set in the inspector");
        }
    }

    private void ButtonFiller()
    {
        if (_btnLabel != null && _btnLabel != "")
        {
            _btnTxt.text = _btnLabel;
            gameObject.name = _btnLabel + "_btn";

            if(_btnLabel == CONSTS.BACK_OPT)
            {
                targetDetail = SubMenuDetail.Back;
            }
        }

        if (!_btn.interactable)
        {
            _btnTxt.color = Color.white;
        }
    }
}