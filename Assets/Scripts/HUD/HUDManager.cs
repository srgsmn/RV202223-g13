using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Utilities;

public class HUDManager : MonoBehaviour
{
    [SerializeField][ReadOnlyInspector] Mode activeMode;

    [Header("Notifications:")]
    [SerializeField] GameObject EPMsg;
    [SerializeField] GameObject navMsg, editMsg, planMsg, generic;
    [SerializeField][ReadOnlyInspector] GameObject activeMsg;

    [Header("Inventory and tips:")]
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject EPSelectTips, NavTips;
    [SerializeField] GameObject[] EditTips;
    [SerializeField] GameObject PlanTips;
    [SerializeField][ReadOnlyInspector] GameObject activeTip;

    [Header("Other UI elements:")]
    [SerializeField] Image frame;
    [SerializeField] Image vf;
    [SerializeField] Image MsgsBg, TipsBg, InventoryBg;
    [SerializeField][ReadOnlyInspector] Color32 EPColor = new Color32(55, 215, 250, 255);
    [SerializeField][ReadOnlyInspector] Color32 NavColor = new Color32(235, 250, 255, 255);
    [SerializeField][ReadOnlyInspector] Color32 EditColor = new Color32(245, 80, 40, 255);
    [SerializeField][ReadOnlyInspector] Color32 PlanColor = new Color32(0, 210, 80, 255);


    /*
    [SerializeField] GameObject firstButton;
    [SerializeField][ReadOnlyInspector] EventSystem eventSystem;
    */

    private void Awake()
    {
        EventsSubscriber();

        //eventSystem = EventSystem.current;
    }

    private void Start()
    {
        ResetNotifications();
        ResetTips();

        ChangeMode(Mode.EPSelector);
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            InputManager.OnChangeMode += ChangeMode;
            InputManager.OnSelection += OnSelection;
            InputManager.OnRotate += OnRotate;
            InputManager.OnTranslate += OnTranslate;
            InputManager.OnBack += CancelAction;
            InputManager.OnConfirm += ConfirmAction;
        }
        else
        {
            InputManager.OnChangeMode -= ChangeMode;
            InputManager.OnSelection -= OnSelection;
            InputManager.OnRotate -= OnRotate;
            InputManager.OnTranslate -= OnTranslate;
            InputManager.OnBack -= CancelAction;
            InputManager.OnConfirm -= ConfirmAction;
        }
    }

    private void SetVFActive(bool flag = true) {
        vf.gameObject.SetActive(flag);
    }

    private void ChangeMode(Mode mode)
    {
        Debug.Log($"{GetType().Name}.cs > CHANGING mode to {mode}");

        activeMode = mode;

        if (activeMode == Mode.Edit)
        {
            SetVFActive();
        }

        ChangeColors(mode);
        DisplayMessage(mode);
        DisplayTips(mode);
    }

    private void ResetNotifications()
    {
        Debug.Log($"{GetType().Name}.cs > RESET notifications");

        navMsg.SetActive(false);
        editMsg.SetActive(false);
        planMsg.SetActive(false);
        generic.SetActive(false);
        MsgsBg.gameObject.SetActive(false);
    }

    private void ResetTips()
    {
        Debug.Log($"{GetType().Name}.cs > RESET tips and inventory");
    }

    private void ChangeColors(Mode mode)
    {
        Color color = Color.white;

        switch (mode)
        {
            case Mode.EPSelector:
                color = EPColor;

                break;
            case Mode.Nav:
                frame.color = new Color32(255, 255, 255, 0);
                color = NavColor;

                break;

            case Mode.Edit:
                color = EditColor;

                break;

            case Mode.Plan:
                color = PlanColor;

                break;
        }

        if (mode != Mode.Nav)
            frame.color = color;

        MsgsBg.color = color;
        TipsBg.color = color;
    }

    private void DisplayMessage(Mode mode)
    {
        MsgsBg.gameObject.SetActive(true);

        switch (mode)
        {
            case Mode.EPSelector:
                activeMsg = EPMsg;

                break;
            case Mode.Nav:
                activeMsg = navMsg;

                break;

            case Mode.Edit:
                activeMsg = editMsg;

                break;

            case Mode.Plan:
                activeMsg = planMsg;

                break;
        }

        if (activeMsg != null)
        {
            activeMsg.SetActive(true);

            StartCoroutine(HideMsg());
        }
    }

    IEnumerator HideMsg()
    {
        yield return new WaitForSeconds(3);

        MsgsBg.gameObject.SetActive(false);
        activeMsg.SetActive(false);
    }

    private void DisplayTips(Mode mode)
    {
        if (mode != Mode.Plan)
        {
            TipsBg.gameObject.SetActive(true);
        }
        else
        {
            TipsBg.gameObject.SetActive(false);
        }

        switch (mode)
        {
            case Mode.EPSelector:
                activeTip = EPSelectTips;

                break;
            case Mode.Nav:
                activeTip = NavTips;

                break;

            case Mode.Edit:
                activeTip = EditTips[0];

                break;

            case Mode.Plan:
                activeTip = inventory;

                break;
        }

        if (activeTip != null)
        {
            activeTip.SetActive(true);
        }
    }

    private void OnSelection()
    {
        GameObject buff = null;

        switch (activeMode)
        {
            case Mode.Edit:
                buff = EditTips[1];

                SetVFActive(false);

                break;

            case Mode.Plan:
                buff = PlanTips;

                break;
        }

        if (buff != null)
        {
            activeTip.SetActive(false);
            activeTip = buff;
            activeTip.SetActive(true);
        }
    }

    private void OnRotate(RotDir dir)
    {
        if (activeMode == Mode.Edit)
        {
            activeTip.SetActive(false);
            activeTip = EditTips[2];
            activeTip.SetActive(true);
        }
    }

    private void OnTranslate(TranDir dir)
    {
        if (activeMode == Mode.Edit)
        {
            activeTip.SetActive(false);
            activeTip = EditTips[3];
            activeTip.SetActive(true);
        }
    }

    private void CancelAction()
    {
        ChangeMode(activeMode);
    }

    private void ConfirmAction()
    {
        switch (activeMode)
        {
            case Mode.Edit:

                ChangeMode(Mode.Edit);

                break;
        }
    }
    /*
    private void ActivateInventory()
    {
        Debug.Log($"{GetType().Name}.cs > ACTIVATING inventory");

        inventory.SetActive(true);

        //eventSystem.firstSelectedGameObject = firstButton;

        //eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }
    */
}
