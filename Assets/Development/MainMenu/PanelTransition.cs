using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelTransition : MonoBehaviour
{
    enum TransitionType { SlideInOut }
    enum TransitionDirection { Top, Right, Bottom, Left}

    [SerializeField] private TransitionType transitionType;
    [SerializeField] private TransitionDirection transitionDirection;

    private RectTransform defRT, startRT, actualRT;
    private Vector3 defPos, startPos, actualPos;

    private TransitionType transType {
        get { return transitionType; }
        set { transitionType = value; }
    }

    private TransitionDirection transDir
    {
        get { return transitionDirection; }
        set { transitionDirection = value; }
    }

    private void Awake()
    {
        defRT = GetComponent<RectTransform>();
        defPos = defRT.position;
        Debug.Log($"{GetType().Name}.cs > Panel default position is {defPos}");

        actualRT = defRT;
        actualPos = defPos;
        Debug.Log($"{GetType().Name}.cs > Panel actual position is {actualPos}");
    }

    private void Start()
    {
        switch (transDir)
        {
            case TransitionDirection.Top:
                startPos.y = defPos.y + defRT.sizeDelta.y;
                break;

            case TransitionDirection.Right:
                startPos.x = defPos.x + defRT.sizeDelta.x;

                break;

            case TransitionDirection.Bottom:
                startPos.y = 2*Screen.height - defPos.y - defRT.sizeDelta.y;

                break;

            case TransitionDirection.Left:
                startPos.x = 2 * Screen.width - defPos.x - defRT.sizeDelta.x;

                break;
        }
    }

    public void SlideOut()
    {

    }

    public void SlideIn()
    {

    }
}
