using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;

public class StartDetailPanel : MonoBehaviour
{
    [SerializeField] StartDetail _detailType;

    Animator anim;

    private void Awake()
    {
        EventsSubscriber();

        anim = GetComponent<Animator>();

        if (anim == null)
        {
            Debug.LogWarning($"{GetType().Name}.cs > Animator component is missing");
        }
    }

    void Start()
    {
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
            StartButton.OnDisplayDetails += DisplaySelf;
        }
        else
        {
            // Events to unsubscribe
            StartButton.OnDisplayDetails -= DisplaySelf;
        }
    }

    private void DisplaySelf(StartDetail target)
    {
        if(_detailType != target && anim.GetBool(CONSTS.ANIM_FLAG))
        {
            anim.SetBool(CONSTS.ANIM_FLAG, false);
        }

        if(_detailType == StartDetail.None)
        {
            Debug.LogWarning($"{GetType().Name}.cs > There is no detail type set in the inspector");
        }

        if(_detailType == target && !anim.GetBool(CONSTS.ANIM_FLAG))
        {
            anim.SetBool(CONSTS.ANIM_FLAG, true);
        }
    }
}
