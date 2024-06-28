using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;
using UnityEditor.Build.Content;

public class MMO : MonoBehaviour
{
    
    public static MMO instance;
    public TextMeshProUGUI ModeDisplay;
    StreamWriter writer;
    EventProcessor logProcessor = new EventProcessor();

    private const float updInterv = 0.05f;
    private float lastUpd = 0f;
    private string tofinoMode;

    void Awake()
    {

        if(MMO.instance == null)
            MMO.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    void Start()
    {

        string pathW = Application.dataPath + "/Docs" + "/saved Data" + "/MMO.txt";
        writer = new StreamWriter(pathW, true);


    }

    void Update()
    {

        lastUpd += Time.deltaTime;

        if(lastUpd >= updInterv)
        {
            
            updMode();
            lastUpd = 0f;

        }

    }

    public void newMode(string logLine)
    {

        string date = logProcessor.getDate(logLine);
        tofinoMode = logProcessor.getMode(logLine);

        saveMode(tofinoMode, date);

    }

    private void updMode()
    {

        ModeDisplay.text = "Mode: " + tofinoMode;

    }

    private void saveMode(string logLine, string date)
    {

        writer.WriteLine(date + " " + tofinoMode);

    }

    void OnDestroy()
    {

        writer.Dispose();

    }


}
