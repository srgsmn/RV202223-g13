using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MenuUtilities;

public class ScreenTransitionController : MonoBehaviour
{
    [SerializeField] Animator _sidebar, _caption, _details, _alert;

    private void Awake()
    {
        DoChecks();
    }

    public void Open()
    {
        Debug.Log($"{GetType().Name}.cs > OPENING {gameObject.name} screen");

        if (_sidebar != null) _sidebar.SetBool(CONSTS.ANIM_FLAG, true);
        if (_caption!=null) _caption.SetBool(CONSTS.ANIM_FLAG, true);
        if (_details != null) _details.SetBool(CONSTS.ANIM_FLAG, true);
        if (_alert != null) _alert.SetBool(CONSTS.ANIM_FLAG, true);
    }

    public void Close()
    {
        Debug.Log($"{GetType().Name}.cs > CLOSING {gameObject.name} screen");

        if (_sidebar != null) _sidebar.SetBool(CONSTS.ANIM_FLAG, false);
        if (_caption != null) _caption.SetBool(CONSTS.ANIM_FLAG, false);
        if (_details != null) _details.SetBool(CONSTS.ANIM_FLAG, false);
        if (_alert != null) _alert.SetBool(CONSTS.ANIM_FLAG, false);
    }

    public void Show(bool flag = true)
    {
        gameObject.SetActive(flag);
    }

    private void DoChecks()
    {
        bool allNull = true;

        if (_sidebar != null) allNull = false;
        if (_caption != null) allNull = false;
        if (_details != null) allNull = false;
        if (_alert != null) allNull = false;

        if (allNull)
        {
            Debug.LogError($"{GetType().Name}.cs > There is no panel linked to this component! At least one should be present in order to work properly!");
        }
    }
}
