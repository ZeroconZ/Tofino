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

    //Text display
    public TextMeshProUGUI AllEvents;
    public TextMeshProUGUI ModbusEvents;
    public TextMeshProUGUI SystemEvents;
    public TextMeshProUGUI ICMPEvents;

    EventProcessor logProcessor = new EventProcessor();

    //Stringbuilders
    StringBuilder AllMsgSB = new StringBuilder(100);
    StringBuilder ModbusSB = new StringBuilder(100);
    StringBuilder ICMPSB = new StringBuilder(100);
    StringBuilder SystemSB = new StringBuilder(100);

    //Queues for each event visualizer
    Queue<string> AllMessages;
    Queue<string> ModbusMessages;
    Queue<string> ICMPMessages;
    Queue<string> SystemMessages;

    //Elements to toggle ACL modbus events
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

        string proLine = logProcessor.eventProcessor(line);     

        int protoNum = logProcessor.getProtocol(line);

        string src = logProcessor.whoIs(logProcessor.getSrcIP(line));
        string dst = logProcessor.whoIs(logProcessor.getDstIP(line));
        string idS = id.ToString();

        AllText(proLine, idS);   

        switch(protoNum)
        {

            case 0:

                ModbusText(line, src, dst, idS);
                break;

            case 2:

                ICMPText(line, src, dst, idS);
                break;

            case 3:

                if(Regex.IsMatch(line, "System") || Regex.IsMatch(logProcessor.getError(line), "Logger"))
                    SystemText(line, idS);    

                break;

        }

    }
    
    public void ACLModbusVisibility()
    {

        if(ViewModbusACLEvents == false)
        {

            ToggleACL.image.sprite = ACLView;
            ViewModbusACLEvents = true;

        }
        else   
        {

            ToggleACL.image.sprite = noACLView;
            ViewModbusACLEvents = false;

        }

    }

    private void AllText(string line, string id)
    {

        AllMsgSB.Append(id)
                .Append(" ")
                .AppendLine(line);

        AllMessages.Enqueue(AllMsgSB.ToString());

        AllMsgSB.Clear();

        foreach(string Event in AllMessages)
            AllMsgSB.AppendLine(Event);

        AllEvents.text = AllMsgSB.ToString();

        AllMsgSB.Clear();

    }

    private void ModbusText(string line, string src, string dst, string id)
    {

        if(Regex.IsMatch(line, "Tofino Modbus/TCP Enforcer"))
        {

            ModbusSB.Append(id + "|")
                       .Append(logProcessor.getError(line) + "|")
                       .Append("source: " + src)
                       .AppendLine(", destination: " + dst);
            ModbusEvents.text = ModbusSB.ToString();

        }
        else if(ViewModbusACLEvents && Regex.IsMatch(line, "ACL"))
        {

            ModbusSB.Append(id + "|")
                       .Append("ACL Violation|")
                       .Append("source: " + src + ", ")
                       .AppendLine("destination: " + dst);
            ModbusEvents.text = ModbusSB.ToString();

        }
        else if(!Regex.IsMatch(line, "ACL"))
        {

            ModbusSB.Append(id + "|")
                       .Append(logProcessor.getError(line) + "|")
                       .Append("source: " + src + ", ")
                       .AppendLine("destination: " + dst);
            ModbusEvents.text = ModbusSB.ToString();         

        }


        return;

    }

    private void ICMPText(string line, string src, string dst, string id)
    {

        if(Regex.IsMatch(line, "incorrect network address"))
        {

            ICMPSB.Append(id + "|")
                    .Append(src)
                    .Append(" cannot reach ")
                    .AppendLine(dst);

            ICMPEvents.text = ICMPSB.ToString();

        }
        else
        {

            ICMPSB.Append(id + "|")
                    .Append(logProcessor.getMsg(line))
                    .Append("source:" +src)
                    .AppendLine(", destination:" + dst);

            ICMPEvents.text = ICMPSB.ToString();

        }

    }

    private void SystemText(string line, string id)
    {

        SystemSB.Append(id + "|")
                  .AppendLine(logProcessor.getError(line));
        
        SystemEvents.text = SystemSB.ToString();

    }

}
