using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SaveParameterManager : MonoBehaviour
{
    [SerializeField] FileManager fileManager;

    [Header("Fields:")]
    [SerializeField] TMP_InputField directoryField;
    [SerializeField] TMP_InputField fileNameField;
    [SerializeField] TMP_Dropdown formatField;

    private void Start()
    {
        directoryField.text = fileManager.GetDirectory();
        fileNameField.text = fileManager.GetFileName();
    }

    public void PasteFromClipboard()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.Paste(); //Copy string from Clipboard to textEditor.text
        directoryField.text = textEditor.text;
    }
}
