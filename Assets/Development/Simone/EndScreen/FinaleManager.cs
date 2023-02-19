using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinaleManager : MonoBehaviour
{
    [SerializeField] GameObject FinaleBtnContainer, SaveAsPanel;

    // Start is called before the first frame update
    void Start()
    {
        SaveAsPanel.SetActive(false);
    }

    public void OpenSaveAs()
    {
        FinaleBtnContainer.SetActive(false);
        SaveAsPanel.SetActive(true);
    }

    public void CloseSaveAs()
    {
        SaveAsPanel.SetActive(false);
        FinaleBtnContainer.SetActive(true);
    }
}
