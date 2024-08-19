using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;
using UnityEditor.MPE;
using System;
using UnityEditor.PackageManager;

public class EventProcessor
{

    StringBuilder processedLine = new StringBuilder();

    public EventProcessor()
    {
    }

    public string getDate(string logLine)
    {

        string datePattern = @"^\w{3}\s{1,2}\d{1,2} \d{2}:\d{2}:\d{2}";
        Match dateMatch = Regex.Match(logLine, datePattern);
        string date = dateMatch.Value;
        Debug.Log(date);

        return date;

    }

    public string getLSM(string logLine)
    {

        string LSMPattern = @"(Tofino Firewall: [^|]+|Tofino ([^\s]+) Enforcer: [^|]+|Tofino Event Logger: [^|]+|Tofino System: [^|]+)";
        Match LSMMatch = Regex.Match(logLine, LSMPattern);
        string LSM = LSMMatch.Value;

        return LSM;

    }

    public string getMsg(string logLine)
    {

        string msgPattern = @"msg=((?!TofinoMode\b).)+";
        Match msgMatch = Regex.Match(logLine, msgPattern);
        string msg = msgMatch.Value;

        return msg;

    }

    public string getSrcIP(string logLine)
    {

        string srcPattern = @"src=([^ ]+)";
        Match srcMatch = Regex.Match(logLine, srcPattern);       
        string src = srcMatch.Value + " ";

        return src;

    }

    public string getDstIP(string logLine)
    {

        string dstPattern = @"dst=([^ ]+)";
        Match dstMatch = Regex.Match(logLine, dstPattern);
        string dst = dstMatch.Value;

        return dst;

    }

    public bool isSrc(string logLine)
    {

        string srcPattern = @"src=([^ ]+)";
        Match srcMatch = Regex.Match(logLine, srcPattern);
        bool srcSucc = srcMatch.Success;
          
        return srcSucc;
                
    }

     public bool isDst(string logLine)
    {

        string dstPattern = @"dst=([^ ]+)";
        Match dstMatch = Regex.Match(logLine, dstPattern);
        bool dst = dstMatch.Success;

        return dst;        

    }

    public string getSMAC(string logLine)
    {

        string smacPattern = @"smac=([0-9a-fA-F]{2}(:[0-9a-fA-F]{2}){5})";
        Match smacMatch = Regex.Match(logLine, smacPattern);
        string smac = smacMatch.Value + " ";

        return smac;

    }

    public string getDMAC(string logLine)
    {

        string dmacPattern = @"dmac=([0-9a-fA-F]{2}(:[0-9a-fA-F]{2}){5})";
        Match dmacMatch = Regex.Match(logLine, dmacPattern);
        string dmac = dmacMatch.Value;

        return dmac;

    }

    public string whoIs(string IP)
    {

        Dictionary<string, string> IPs = new Dictionary<string, string>();

        IPs.Add("10.1.1.10", "PLC");
        IPs.Add("10.1.1.12", "DRIVER");
        IPs.Add("10.1.1.11", "HMI");
        IPs.Add("10.1.1.101", "PC");

        Match IPMatch = Regex.Match(IP, @"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}");
        string IPvalue = IPMatch.Value;

        if(IPs.TryGetValue(IPvalue, out string itIs))
            return itIs;

        else   
        {

            //Debug.Log(IPvalue);
            return IPvalue;

        }

    }

    public string eventProcessor(string logLine)
    {

        processedLine.Clear();
        string proto;

        if(getProtocol(logLine) == 0)
            proto = "proto = MODBUS";
        else if(getProtocol(logLine) == 1)
            proto = "proto = TCP";
        else if(getProtocol(logLine) == 2)
            proto = "proto = ICMP";
        else
            proto = Regex.Match(logLine,@"proto=(\w+)(/(\w+))?").Value;

        string date = getDate(logLine);
        string LSM = getLSM(logLine);
        string msg = getMsg(logLine);

        if(!isSrc(logLine) || !isDst(logLine))
        {

            string smac = getSMAC(logLine);
            string dmac = getDMAC(logLine);

            processedLine.Append(date + "|")
                         .Append(LSM + "|")
                         .Append(msg + " ")
                         .Append(smac + " ")
                         .Append(dmac + " ")
                         .AppendLine(proto);
                     
        }
        else
        {

            string src = getSrcIP(logLine);
            string dst = getDstIP(logLine);

            processedLine.Append(date + "|")
                         .Append(LSM + "|")
                         .Append(msg + " ")
                         .Append(src + " ")
                         .Append(dst + " ")
                         .AppendLine(proto);          

        }

        return processedLine.ToString();

    }

    public bool getModeChange(string logLine)
    {

        string modePattern = @"(Tofino System: [^|]+)";
        Match modeMatch = Regex.Match(logLine, modePattern);
        string mode = modeMatch.Value;

        if(mode.Contains("Change"))
            return true;
        
        else
            return false;

    }

    public string getMode(string logLine)
    {
        
        string OPPattern = "OPERATIONAL";
        string TEPattern = "TEST";

        if(Regex.IsMatch(logLine, OPPattern))

            return "OPERATIONAL";

        else if(Regex.IsMatch(logLine, TEPattern))

            return "TEST";

        else

            return "ERROR";

    }

    public string getError(string logLine)
    {

        string ACLPattern = @"ACL";
        string ModbusPattern = @"Modbus/TCP";
        string EventPattern = @"Event Logger";
        string SystemPattern = @"System:";

        if(Regex.IsMatch(logLine, ACLPattern))
        {

            return "ACL Error";

        }
        else if(Regex.IsMatch(logLine, ModbusPattern))
        {

            string patternCode = @"code (\d{1,2})";
            Match codeMatch = Regex.Match(logLine, patternCode);
            string code = codeMatch.Value;

            string patternError = @"\d{1,2}";
            Match errorMatch = Regex.Match(code, patternError);
            int error = Int32.Parse(errorMatch.Value);

            code = modbusErrors(error);

            return code;

        }
        else if(Regex.IsMatch(logLine, EventPattern))
        {

            return "Event Logger connected";

        }
        else if(Regex.IsMatch(logLine, SystemPattern))
        {

            string systemEventPattern = @"Tofino System: ([^|]+)";
            Match systemEventMatch = Regex.Match(logLine, systemEventPattern);
            string systemEvent = systemEventMatch.Groups[1].Value;

            return systemEvent;

        }
        else
        {

            return "Unknown Error";

        }

    }

    public int getProtocol(string logLine)
    {

        string protoPattern = @"proto=(\w+)/(\w+)";
        Match protoMatch = Regex.Match(logLine, protoPattern);
        string proto = protoMatch.Groups[2].Value;

        if(proto == "TCP" && Regex.IsMatch(logLine, "dpt=502")) //Modbus
        {

            return 0;

        }
        else if(proto == "TCP" ) //TCP
        { 

            return 1;

        }
        else if(proto == "ICMP") //ICMP
        {
            
            return 2;

        }
        else //Genérico
        {

            return 3;

        }
        
    }

    private string modbusErrors(int code)
    {

        switch(code)
        {

            case 1:
                return "Reading Coils";
            
            case 2: 
                return "Reading Discrete Inputs";

            case 3:
                return "Reading Holding Registers";

            case 4:
                return "Reading Input Registers";

            case 5:
                return "Writing Single Coil";
            case 6:
                return "Writing Single Register";

            case 7:
                return "Reading Exception Status";

            case 8:
                return "Diagnostics";

            case 11:
                return "Getting Comm Event Counter";

            case 12: 
                return "Getting Com Event Log";

            case 15:
                return "Writing Multiple Coils";

            case 16:
                return "Writing Multiple Registers";

            case 17:
                return "Reporting Server ID";

            case 20:
                return "Reading File Record";

            case 21: 
                return "Writing File Record";

            case 22:
                return "Mask Writing Regiser";

            case 23:
                return "Reading/Writing Multiple Registers";

            case 24:
                return "Reading FIFO Queue";

            case 43:
                return "Reading Device Identification";

            case 90:
                return "Unity Programming/OFS";

            default:
                return "Unknown Error";

        }

    }

}
