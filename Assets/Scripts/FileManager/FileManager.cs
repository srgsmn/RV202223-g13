/* LINK UTILI: 
 *      https://prasetion.medium.com/saving-data-as-json-in-unity-4419042d1334
 *      https://answers.unity.com/questions/1397703/system-io-file-directory-problem.html
 *      
 */
using UnityEngine;
using System.IO;
using System;
using Utilities;
using System.Collections;
using System.Collections.Generic;
using static UnityEditor.FilePathAttribute;

public class FileManager : MonoBehaviour
{
    //TODO CANCELLARE
    [Serializable]
    public class ExampleClass
    {
        public int exInt;
        public float exFloat;
        public string[] exArrString;
    }

    [System.Serializable]
    public class AccessibilityReport
    {
        public string sessionID;
        public int eventsTotal;
        public string[] eventsList;
        public int collisionsTotal;
        public string[] collisionsList;
        public int accDevicesTotal;
        public string[] accDevicesList;

        public int translationsTotal;
        public TranslationEntry[] translationsList;

        public AccessibilityReport(string sessionID, List<string> eventsList, List<string> collisionsList, List<string> accDevicesList, Dictionary<string, ReportCreator.RotoTranslation> translationsList)
        {
            this.sessionID = sessionID;

            this.eventsList = eventsList.ToArray();
            eventsTotal = this.eventsList.Length;

            this.collisionsList = collisionsList.ToArray();
            collisionsTotal = this.collisionsList.Length;

            this.accDevicesList = accDevicesList.ToArray();
            accDevicesTotal = this.accDevicesList.Length;

            this.translationsList = new TranslationEntry[translationsList.Count];
            translationsTotal = this.translationsList.Length;

            int i = 0;
            foreach(KeyValuePair<string, ReportCreator.RotoTranslation> entry in translationsList)
            {
                this.translationsList[i++] = new TranslationEntry(entry.Key, entry.Value);
            }
        }

        public override string ToString()
        {
            string intro = $"Session ID = {sessionID}\n\nGENERAL DATA:\n\tTotal events: {eventsTotal}\n\tTotal collisions: {collisionsTotal}\n\tTotal accessibility devices added: {accDevicesTotal}\n\tTotal translations: {translationsTotal}";

            string events = "\n\n## EVENTS HISTORY ##\n\n" + string.Join("\n", eventsList);
            string collisions = "\n\n## COLLISIONS HISTORY ##\n\n" + string.Join("\n", collisionsList);
            string accDevs = "\n\n## ACCESSIBILITY DEVICES HISTORY ##\n\n" + string.Join("\n", accDevicesList);
            string translations = "\n\n## TRANSLATIONS HISTORY ##\n\n" + string.Join("\n", eventsList);

            return intro+events+collisions+accDevs+translations;
        }

        // INNER CLASS
        [System.Serializable]
        public class TranslationEntry
        {
            public string name;
            public RotoTranslation rotoTranslation;

            public TranslationEntry(string name, ReportCreator.RotoTranslation rotoTranslation)
            {
                this.name = name;
                this.rotoTranslation = new RotoTranslation(rotoTranslation.translation, rotoTranslation.rotation);
            }

            public override string ToString()
            {
                return $"Object Name = {name}, Rototranslation: [{rotoTranslation}]";
            }

            // INNER CLASS
            [System.Serializable]
            public class RotoTranslation
            {
                public float[] translation = new float[3];
                public float rotation;

                public RotoTranslation(Vector3 translation, float rotation)
                {
                    this.translation[0] = translation.x;
                    this.translation[1] = translation.y;
                    this.translation[2] = translation.z;

                    this.rotation = rotation;
                }

                public override string ToString()
                {
                    return $"Translation: [x = {translation[0]}, y = {translation[1]}, z = {translation[2]}], Rotation: {rotation}";
                }
            }
        }
    }

    private enum FileFormat { TXT = 0, JSON }

    [Header("Save info:")]
    [SerializeField][ReadOnlyInspector] private string sessionID;
    [SerializeField] private string path;
    [SerializeField] private string fileNameBase = "WayFinder_Report_";
    [SerializeField][ReadOnlyInspector] string fileName;
    [SerializeField][ReadOnlyInspector] string dataPath;
    [SerializeField][ReadOnlyInspector] FileFormat fileFormat;

    [Header("Accessibility Report:")]
    [SerializeField] ReportCreator reportCreator;
    [SerializeField] AccessibilityReport accRep;

    //TODO
    ExampleClass _test;

    private void Awake()
    {
        if (!GameObject.Find("ReportCreator").TryGetComponent<ReportCreator>(out reportCreator))
        {
            Debug.LogError("### REPORT CREATOR IS MISSING");
        }
    }

    private void Start()
    {
        path = Application.dataPath;

        sessionID = GameManager.Instance.GetSessionID();

        fileName = fileNameBase + sessionID;

        //TODO
        _test = new ExampleClass();
        //TODO Decommentare sotto e sostituire a _test
        //accRep = new AccessibilityReport(sessionID, reportCreator._allRecords, reportCreator._allCollisions, reportCreator._allAccDevices, reportCreator._allTranslations);
    }

    public void SaveReport()
    {
        string data ="";

        switch (fileFormat)
        {
            case FileFormat.TXT:
                //data = DataToTxt(_test);    //TODO cambiare con accRep

                break;

            case FileFormat.JSON:
                data = JsonUtility.ToJson(_test);   //TODO cambiare con accRep
                //data = JsonUtility.ToJson(accRep);

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

    /// <summary>
    /// Transform data to a string with "\t" chars. To avoid them, set the parameter to true (false by default)
    /// </summary>
    /// <param name="toDisplay">flag that if it's true removes "\t" chars if the string should be shown in screen, otherwise keeps them (false by default)</param>
    /// <returns></returns>
    public string DataToTxt(bool toDisplay=false)
    {
        string txt = $"WAYFINDER ACCESSIBILITY REPORT\n";

        if (accRep == null)
            txt += "";
        else
            txt += accRep.ToString();

        if (toDisplay)
            txt.Replace("\t", "");

        return txt;
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

    public string GetDirectory()
    {
        return path;
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
