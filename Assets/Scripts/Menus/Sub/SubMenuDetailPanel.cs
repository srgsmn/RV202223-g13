using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;

public class SubMenuDetailPanel : MonoBehaviour
{
    [SerializeField] SubMenuDetail _detailType;
    [SerializeField] [Tooltip("To set on false to avoid useless warnings")] bool _mustHideButtons = false;
    [SerializeField] GameObject _detailsContainer;

    Animator anim;

    private void Awake()
    {
        EventsSubscriber();

        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Animator component is MISSING");
        }

        if (_detailsContainer == null && _mustHideButtons)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Details container is MISSING");
        }
    }

    private void OnDestroy()
    {
        EventsSubscriber(false);
    }

    private void EventsSubscriber(bool subscribing = true)
    {
        if (subscribing)
        {
            // Event to subscribe
            SubMenuButton.OnDisplayDetails += DisplaySelf;
        }
        else
        {
            // Events to unsubscribe
            SubMenuButton.OnDisplayDetails -= DisplaySelf;
        }
    }

    private void DisplaySelf(SubMenuDetail target)
    {
        if(_detailType != target && anim.GetBool(CONSTS.ANIM_FLAG))
        {
            anim.SetBool(CONSTS.ANIM_FLAG, false);
            if (_detailsContainer != null)
                _detailsContainer.SetActive(false);
        }

        if(_detailType == SubMenuDetail.None)
        {
            Debug.LogWarning($"{GetType().Name}.cs > There is no detail type set in the inspector");
        }

        if(_detailType == target && !anim.GetBool(CONSTS.ANIM_FLAG))
        {
            if(_detailsContainer != null)
                _detailsContainer.SetActive(true);
            anim.SetBool(CONSTS.ANIM_FLAG, true);
        }
    }
}
