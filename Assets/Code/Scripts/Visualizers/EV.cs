using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.Text;
using System.Text.RegularExpressions;


public class EventVis : MonoBehaviour
{

    public static EventVis instance;
    public TextMeshProUGUI AllEvents;
    public TextMeshProUGUI ModbusEvents;
    public TextMeshProUGUI SystemEvents;
    public TextMeshProUGUI ICMPEvents;

    EventProcessor logProcessor = new EventProcessor();
    StringBuilder logLineConc = new StringBuilder();
    StringBuilder ModbusError = new StringBuilder();
    StringBuilder ICMPError = new StringBuilder();
    StringBuilder SystemInfo = new StringBuilder();

    public UnityEngine.UI.Button ToggleACL;
    private bool ViewModbusACLEvents = true;
    public Sprite ACLView, noACLView;
    

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
                
                if(Regex.IsMatch(line, "ACL"))
                    Debug.Log("ES ACL");

                ModbusText(line, src, dst, id.ToString());
                break;

            case 2:

                ICMPText(line, src, dst, id.ToString());
                break;

            case 3:

                if(Regex.IsMatch(logProcessor.getError(line), "System") || Regex.IsMatch(logProcessor.getError(line), "Logger"))
                    SystemText(line, id.ToString());    

                break;

        }

        updText();

    }

    private void updText()
    {

        AllEvents.text = logLineConc.ToString();

    }
    
    public void ACLModbusVisibility()
    {

        if(ViewModbusACLEvents == false)
        {

            ToggleACL.image.sprite = ACLView;
            ViewModbusACLEvents = true;
            Debug.Log("ACL on");

        }
        else   
        {

            ToggleACL.image.sprite = noACLView;
            ViewModbusACLEvents = false;
            Debug.Log("ACL off");

        }

    }

    private void ModbusText(string line, string src, string dst, string id)
    {

        if(Regex.IsMatch(line, "Tofino Modbus/TCP Enforcer"))
        {

            ModbusError.Append(id + "|")
                       .Append(src)
                       .Append(" cannot " + logProcessor.getError(line) + " from ")
                       .AppendLine(dst);
            Debug.Log("Error funcion no permitida");
            ModbusEvents.text = ModbusError.ToString();

        }
        else if(ViewModbusACLEvents && Regex.IsMatch(line, "ACL"))
        {

            ModbusError.Append(id + "|")
                       .Append("ACL Violation|")
                       .Append("source: " + src + ", ")
                       .AppendLine("destination: " + dst);
            ModbusEvents.text = ModbusError.ToString();

        }


        return;

    }

    private void ICMPText(string line, string src, string dst, string id)
    {

        if(Regex.IsMatch(line, "incorrect network address"))
        {

            ICMPError.Append(id + "|")
                    .Append(src)
                    .Append(" cannot reach ")
                    .AppendLine(dst);

            ICMPEvents.text = ICMPError.ToString();

        }
        else
        {

            ICMPError.Append(id + "|")
                    .Append(logProcessor.getMsg(line))
                    .Append("source:" +src)
                    .AppendLine(", destination:" + dst);

            ICMPEvents.text = ICMPError.ToString();

        }


    }

    private void SystemText(string line, string id)
    {

        SystemInfo.Append(id + "|")
                  .AppendLine(logProcessor.getError(line));
        
        SystemEvents.text = SystemInfo.ToString();

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
