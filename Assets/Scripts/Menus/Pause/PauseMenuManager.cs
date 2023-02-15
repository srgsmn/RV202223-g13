using System.Collections;
using System.Collections.Generic;
using MenuUtilities;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenuManager : MonoBehaviour
{
    [SerializeField] GameObject sidebar, quitAlert;
    [SerializeField] Animator _animator;
    [Header("First Buttons:")]
    [SerializeField] GameObject sbBtn1st;
    [SerializeField] GameObject alBtn1st;
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

        eventSystem.firstSelectedGameObject = sbBtn1st;

        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void OpenAlert()
    {
        sidebar.SetActive(false);
        quitAlert.SetActive(true);

        eventSystem.firstSelectedGameObject = alBtn1st;

        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void CloseAlert()
    {
        quitAlert.SetActive(false);
        sidebar.SetActive(true);

        eventSystem.firstSelectedGameObject = sbBtn1st;

        eventSystem.SetSelectedGameObject(eventSystem.firstSelectedGameObject);
    }

    public void QuitApp()
    {
        GameManager.Instance.QuitApp();
    }
}
