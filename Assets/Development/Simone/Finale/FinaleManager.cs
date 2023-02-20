using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FinaleManager : MonoBehaviour
{
    [SerializeField] GameObject FinaleBtnContainer, SaveAsPanel;
    [SerializeField] TextMeshProUGUI reportArea;

    // Start is called before the first frame update
    void Start()
    {
        SaveAsPanel.SetActive(false);

        LoadReport();
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

    private void LoadReport()
    {
        string data = "";
        int lines = 0;

        //TODO

        reportArea.text = data;

        reportArea.rectTransform.sizeDelta = new Vector2(1000, lines * reportArea.fontSize);
    }
}
