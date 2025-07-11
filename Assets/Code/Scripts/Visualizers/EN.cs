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
using System;
using System.Text.RegularExpressions;

public class EventNotif : MonoBehaviour
{

    public static EventNotif instance;
    public TextMeshProUGUI ErrorNotif;
    public TextMeshProUGUI MoreInfo;

    EventProcessor logProcessor = new EventProcessor();

    StringBuilder error = new StringBuilder();
    private string id;

    void Awake()
    {

        if(EventNotif.instance == null)
            EventNotif.instance = this;

        else 
            DestroyImmediate(gameObject);

    }       

    public void newNotif(string line, string idS)
    {
        
        string header = string.Empty;

        if(Regex.IsMatch(line, " allowed and logged as specified in the ACL"))
        {

            header = "Event Allowed";

            error.AppendLine(header)
                 .AppendLine("Source: " + logProcessor.whoIs(logProcessor.getSrcIP(line)))
                 .Append("Destination: " + logProcessor.whoIs(logProcessor.getDstIP(line)));

        }
        else
        {
            if(Regex.IsMatch(line, "System") || Regex.IsMatch(logProcessor.getError(line), "Logger"))
            {

                error.Append(logProcessor.getError(line));

            }
            else
            {

                if(!logProcessor.isSrc(line) || !logProcessor.isDst(line))
                {

                    error.AppendLine(logProcessor.getError(line))
                         .AppendLine("Source: unknown")
                         .Append("Destination: unknown");

                }
                else
                {

                    if(logProcessor.getProtocol(line) == 0)
                    {

                        if(Regex.IsMatch(line, "ACL"))
                            header = "Modbus ACL Error";

                        else if(Regex.IsMatch(line, "Tofino Modbus/TCP Enforcer"))
                            header = logProcessor.getError(line);

                    }


                    
                    else if(logProcessor.getProtocol(line) == 2)
                        header = "ICMP Error";
                    
                    else 
                        header = logProcessor.getError(line);   

                    error.AppendLine(header)
                        .AppendLine("Source: " + logProcessor.whoIs(logProcessor.getSrcIP(line)))
                        .Append("Destination: " + logProcessor.whoIs(logProcessor.getDstIP(line)));

                }

            }

        }

        id = idS;

        updText();

    }

    private void updText()
    {

        ErrorNotif.text = error.ToString();
        MoreInfo.text = "\nRead line " + id + " for more information";

        error.Clear();

    }

}
