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
    StringBuilder ICMPError = new StringBuilder();

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

        string src = logProcessor.whoIs(logProcessor.getSrcIP(line));
        string dst = logProcessor.whoIs(logProcessor.getDstIP(line));

        switch(protoNum)
        {

            case 0:
                
                ModbusText(line, src, dst, id.ToString());
                Debug.Log("ES MODBUS");
                break;

            case 2:

                ICMPText(line, src, dst, id.ToString());
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
    
    private void ModbusText(string line, string src, string dst, string id)
    {

        if(Regex.IsMatch(line, "ACL"))
        {

            ModbusError.Append(id + "|")
                       .Append("ACL Violation|")
                       .Append("source: " + src + ", ")
                       .AppendLine("destination: " + dst);

            ModbusEvents.text = ModbusError.ToString();

        }
        else
        {

            ModbusError.Append(id + "|")
                       .Append(src)
                       .Append(" cannot " + logProcessor.getError(line) + " from ")
                       .AppendLine(dst);

        }

    }

    private void ICMPText(string line, string src, string dst, string id)
    {

        ICMPError.Append(id + "|")
                 .Append(src)
                 .Append(" cannot reach ")
                 .Append(dst);

        ICMPEvents.text = ICMPError.ToString();

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
