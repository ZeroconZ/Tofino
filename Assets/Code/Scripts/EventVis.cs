using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;

public class EventVis : MonoBehaviour
{

    public static EventVis instance;
    public TextMeshProUGUI TextOnS;
    StringBuilder logLineConc = new StringBuilder();

    private const float updInterv = 0.5f;
    private float lastUpd = 0f;
    private string logLine;

    void Awake()
    {

        if(EventVis.instance == null)
            EventVis.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    void Update()
    {

        lastUpd += Time.deltaTime;

        if(lastUpd >= updInterv)
        {
            
            updText();
            lastUpd = 0f;
            logLine = "";

        }

    }

    public void newLog(string line)
    {
         
        logLine = line;
        logLineConc.Append(line);

    }

    private void updText()
    {

        TextOnS.text = TextOnS.text + logLine;

    }

}
