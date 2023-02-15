using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertBtn : MonoBehaviour
{
    [SerializeField] PauseMenuManager pmm;

    public void Cancel()
    {
        pmm.CloseAlert();
    }

    public void Quit()
    {
        pmm.QuitApp();
    }
}
