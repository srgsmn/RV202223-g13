/* LINK UTILI: 
 *      https://prasetion.medium.com/saving-data-as-json-in-unity-4419042d1334
 *      https://answers.unity.com/questions/1397703/system-io-file-directory-problem.html
 *      
 */
using UnityEngine;
using System.IO;
using System;
using Utilities;

public class FileManager : MonoBehaviour
{
    [Serializable]
    public class ExampleClass
    {
        public int exInt;
        public float exFloat;
        public string[] exArrString;
    }

    private enum FileFormat { TXT=0, JSON }

    [SerializeField] private string path;
    [SerializeField] private string fileNameBase = "WayFinder_Report_";
    [SerializeField][ReadOnlyInspector] string fileName;
    [SerializeField][ReadOnlyInspector] string dataPath;
    [SerializeField][ReadOnlyInspector] FileFormat fileFormat;

    ExampleClass _test;

    private void Start()
    {
        fileName = fileNameBase + GameManager.Instance.GetSessionID();

        //TODO
        _test = new ExampleClass();
    }

    public void SaveReport()
    {
        string data ="";

        switch (fileFormat)
        {
            case FileFormat.TXT:
                data = DataToTxt(_test);

                break;

            case FileFormat.JSON:
                data = JsonUtility.ToJson(_test);

                break;
        }

        try
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            dataPath = $"{path}/{fileName}.{FileFormatToString()}";

            File.WriteAllText(dataPath, data);

        }
        catch (Exception ex)
        {
            string ErrorMessages = "File Write Error\n" + ex.Message;

            Debug.LogError(ErrorMessages);
        }
    }

    private string DataToTxt(ExampleClass example)
    {
        string data = $"WAYFINDER REPORT ({GameManager.Instance.GetSessionTimestamp()})";

        //TODO

        data += $"\nexInt: {example.exInt}";
        data += $"\nexFloat: {example.exFloat}";
        data += $"\nexArrString:";

        foreach(string entry in example.exArrString)
        {
            data += "\n\t" + entry;
        }

        return data;
    }

    public string GetFileName()
    {
        return fileName;
    }

    public void SetFileName(string fileName)
    {
        this.fileName = fileName;
    }

    public void SetDirectory(string directory)
    {
        this.path = directory;
    }

    public void SetFileFormat(int index)
    {
        fileFormat = (FileFormat)index;
    }

    public string FileFormatToString()
    {
        string format = "";

        switch (fileFormat)
        {
            case FileFormat.TXT:

                format = ".txt";

                break;

            case FileFormat.JSON:

                format = ".json";

                break;
        }

        return format;
    }
}
