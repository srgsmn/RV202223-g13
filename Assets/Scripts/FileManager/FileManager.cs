using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Data;
using Utilities;

public class FileManager : MonoBehaviour
{
    private int _totalDistance = 0; //TODO

    [SerializeField] private string fileNameBase = "WayFinder_Report_";
    [SerializeField][ReadOnlyInspector] string fileName;

    private void Start()
    {
        fileName = fileNameBase + GameManager.Instance.GetSessionID();
    }

    public void SaveReport()
    {
        /*
        string path = EditorUtility.SaveFolderPanel("Save the report to folder...", "", "");

        if (path.Length != 0)
        {
            //TODO
            /*
            StreamWriter writer = new StreamWriter(path+"/"+fileName, true);
            foreach (string a in _allRecords)
            {
                writer.WriteLine(a + "; \n");
            }

            writer.WriteLine("Distanza totale: " + _totalDistance + "; \n");
            writer.Close();
            
        }
        */
    }
}
