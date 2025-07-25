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
    StringBuilder AllMsgSB = new StringBuilder();
    StringBuilder ModbusSB = new StringBuilder();
    StringBuilder ICMPSB = new StringBuilder();
    StringBuilder SystemSB = new StringBuilder();

    //Queues for each event visualizer
    Queue<string> AllMessages = new Queue<string>();
    Queue<string> ModbusMessages = new Queue<string>();
    Queue<string> ICMPMessages = new Queue<string>();
    Queue<string> SystemMessages = new Queue<string>();

    //UI elements to toggle ACL modbus events
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

        AllMsgSB.Append(id + "|")
                .AppendLine(line);

        if(AllMessages.Count > 100)
        {

            AllMessages.Dequeue();
            AllMessages.Enqueue(AllMsgSB.ToString());

        }
        else
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
                    .Append(logProcessor.getLSM(line) + "|")
                    .Append(logProcessor.getError(line) + " (" + Regex.Match(line, @"code (\d{1,2})").Value + ")" + " is not permitted, check enforcer rules|")
                    .Append("source: " + src)
                    .AppendLine(", destination: " + dst);

        }
        else if(ViewModbusACLEvents && Regex.IsMatch(line, "ACL"))
        {

            if(Regex.IsMatch(line, " allowed and logged as specified in the ACL"))
            {

                ModbusSB.Append(id + "|")
                        .Append(logProcessor.getLSM(line) + "|")
                        .Append(src + " ")
                        .Append("was able to reach ")
                        .AppendLine(dst);

            }
            else
            {

                ModbusSB.Append(id + "|")
                        .Append(logProcessor.getLSM(line) + "|")
                        .AppendLine(src + " Was not able to reach " + dst  + ", check firewall rules");

            }

            
        }
        else if(!Regex.IsMatch(line, "ACL"))
        {

            ModbusSB.Append(id + "|")
                    .Append(logProcessor.getLSM(line) + "|")
                    .Append(logProcessor.getError(line) + ", check firewall rules|")
                    .Append("source: " + src + ", ")
                    .AppendLine("destination: " + dst);
                   
        }

        if(ModbusMessages.Count > 100)
        {

            ModbusMessages.Dequeue();
            ModbusMessages.Enqueue(ModbusSB.ToString());

        }
        else
            ModbusMessages.Enqueue(ModbusSB.ToString());

        ModbusSB.Clear();

        foreach(string Event in ModbusMessages)
            ModbusSB.AppendLine(Event);

        if(ModbusSB.Length > 30)
            ModbusEvents.text = ModbusSB.ToString();
            
        ModbusSB.Clear();

        return;

    }

    private void ICMPText(string line, string src, string dst, string id)
    {

        if(Regex.IsMatch(line, "incorrect network address"))
        {

            ICMPSB.Append(id + "|")
                    .Append(logProcessor.getLSM(line) + "|")
                    .Append(src)
                    .Append(" cannot reach ")
                    .Append(dst)
                    .AppendLine(", check firewall rules");

        }
        else if(Regex.IsMatch(line, " allowed and logged as specified in the ACL"))
        {

            ICMPSB.Append(id + "|")
                    .Append(logProcessor.getLSM(line) + "|")
                    .Append(src)
                    .Append(" was able to reach ")
                    .AppendLine(dst);

        }
        else
        {

            ICMPSB.Append(id + "|")
                  .Append(logProcessor.getLSM(line) + "|")
                  .Append(logProcessor.getMsg(line))
                  .Append("source:" +src)
                  .AppendLine(", destination:" + dst);

        }

        if(ICMPMessages.Count > 100)
        {

            ICMPMessages.Dequeue();
            ICMPMessages.Enqueue(ICMPSB.ToString());

        }
        else
            ICMPMessages.Enqueue(ICMPSB.ToString());

        ICMPSB.Clear();

        foreach(string Event in ICMPMessages)
            ICMPSB.AppendLine(Event);

        ICMPEvents.text = ICMPSB.ToString();

        ICMPSB.Clear();

    }

    private void SystemText(string line, string id)
    {

        if(logProcessor.getModeChange(line))
        {

            SystemSB.Append(id + "|")
                    .Append(logProcessor.getLSM(line) + "|")
                    .AppendLine("Device mode now is " + logProcessor.getMode(line));

        }
        else if(Regex.IsMatch(line, @"suser=(\w+)"))
        {

            SystemSB.Append(id + "|")
                    .Append(logProcessor.getLSM(line) + "|")
                    .AppendLine("source = " + Regex.Match(line, @"suser=(\w+)").Groups[1].Value);

        }
        else
        {

             SystemSB.Append(id + "|")
                     .AppendLine(logProcessor.getLSM(line));

        }
        

        if(SystemMessages.Count > 100)
        {

            SystemMessages.Dequeue();
            SystemMessages.Enqueue(SystemSB.ToString());

        }
        else
            SystemMessages.Enqueue(SystemSB.ToString());

        SystemSB.Clear();

        foreach(string Event in SystemMessages)
            SystemSB.AppendLine(Event);
        
        SystemEvents.text = SystemSB.ToString();

        SystemSB.Clear();

    }

}
