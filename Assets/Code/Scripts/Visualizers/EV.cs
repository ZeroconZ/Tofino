using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;
using System.Text.RegularExpressions;

public class EventVis : MonoBehaviour
{

    public static EventVis instance;
    public TextMeshProUGUI AllEvents;
    public TextMeshProUGUI ModbusEvents;
    //public TextMeshProUGUI TCPEvents;
    public TextMeshProUGUI ICMPEvents;

    EventProcessor logProcessor = new EventProcessor();
    StringBuilder logLineConc = new StringBuilder();
    StringBuilder ModbusError = new StringBuilder();

    void Awake()
    {

        if(EventVis.instance == null)
            EventVis.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    public void newLog(string line, int id)
    {
        
        if(id % 200 == 0)
            removeOldLines(id);

        string proLine = logProcessor.eventProcessor(line);
        logLineConc.Append(id.ToString())
                   .Append(" ")
                   .AppendLine(proLine);

        int protoNum = logProcessor.getProtocol(line);

        switch(protoNum)
        {

            case 0:
                
                ModbusText(line);
                break;

            case 2:

                ICMPEvents.text = logLineConc.ToString();
                break;

            default:    
                break;

        }

        updText();

    }

    private void updText()
    {

        AllEvents.text = logLineConc.ToString();

    }
    
    private void ModbusText(string line)
    {

        if(Regex.IsMatch(line, "ACL"))
        {

            

        }

    }

    //necesita trabajo
    private void removeOldLines(int id)
    {

        int previousIndex = 0;
        int index = 0;

        for (int i = 0; i < 100; i++)
        {
            index = logLineConc.ToString().IndexOf('\n', previousIndex);
            if (index == -1)
            {

                break;
            }

            logLineConc.Remove(previousIndex, index - previousIndex + 1);
        }
    }

}
