using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject quitAlert;
    [SerializeField] Animator _animator;
    [Header("First Button:")]
    [SerializeField] GameObject firstButton;
    [SerializeField] private EventSystem eventSystem;

    private void Awake()
    {
        _animator = GetComponent<Animator>();

        eventSystem = EventSystem.current;
    }

    private void Start()
    {
        CloseAlert();
    }

    private void OnEnable()
    {
        _animator.SetBool(CONSTS.ANIM_FLAG, true);

        eventSystem.firstSelectedGameObject = firstButton;

        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void OpenAlert()
    {
        quitAlert.SetActive(true);
    }

    public void CloseAlert()
    {
        quitAlert.SetActive(false);
    }

    public void QuitApp()
    {
        GameManager.Instance.QuitApp();
    }
}
