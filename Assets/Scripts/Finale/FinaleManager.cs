using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text.RegularExpressions;

public class FinaleManager : MonoBehaviour
{
    [SerializeField] GameObject FinaleBtnContainer, SaveAsPanel;
    [SerializeField] public TextMeshProUGUI reportArea;
    //[SerializeField] FileManager fm;

    // Start is called before the first frame update
    void Start()
    {
        SaveAsPanel.SetActive(false);

        Debug.Log("### CALLING LoadReport");
        //LoadReport();
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

    /*
    private void LoadReport()
    {
        Debug.Log("### LoadReport():");
        string data = "";

        data = fm.DataToTxt(true);

        int lines = Regex.Matches(data, "\n").Count + 5;

        Debug.Log($"### Data (with {lines} lines is {data}");

        Debug.Log("### TRANSFERRING data to text area.");

        reportArea.text = data;

        reportArea.rectTransform.sizeDelta = new Vector2(1000, lines * reportArea.fontSize);
    }
    */
}
