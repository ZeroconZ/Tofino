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

public class LogProcessor
{

    StringBuilder processedLine = new StringBuilder();

    public LogProcessor()
    {
    }

    public string getDate(string logLine)
    {

        string datePattern = @"^\w{3} \d{2} \d{2}:\d{2}:\d{2}";
        Match dateMatch = Regex.Match(logLine, datePattern);
        string date = dateMatch.Value;

        return date;

    }

    public string lineProcessor(string logLine)
    {

        processedLine.Clear();

        string src;
        string dst;
        string smac;
        string dmac;

        string date = getDate(logLine);

        string LSMPattern = @"(Tofino Firewall: [^|]+|Tofino ([^\s]+) Enforcer: [^|]+|Tofino Event Logger: [^|]+|Tofino System: [^|]+)";
        Match LSMMatch = Regex.Match(logLine, LSMPattern);
        string LSM = LSMMatch.Value;

        string msgPattern = @"msg=((?!TofinoMode\b).)+";
        string srcPattern = @"src=([^ ]+)";
        string dstPattern = @"dst=([^ ]+)";

        Match msgMatch = Regex.Match(logLine, msgPattern);
        Match srcMatch = Regex.Match(logLine, srcPattern);
        Match dstMatch = Regex.Match(logLine, dstPattern);

        string msg = msgMatch.Value;

        if(!srcMatch.Success || !dstMatch.Success)
        {

            string smacPattern = @"smac=((?!dmac=\b).)+";
            string dmacPattern = @"dmac=((?!proto=\b).)+";

            Match smacMatch = Regex.Match(logLine, smacPattern);
            Match dmacMatch = Regex.Match(logLine, dmacPattern);

            smac = smacMatch.Value;
            smac = smac.Trim();
            dmac = dmacMatch.Value;
            dmac = dmac.Trim();

            processedLine.Append(date + "|")
                         .Append(LSM + "|")
                         .Append(msg + "|")
                         .Append(smac +"|")
                         .Append(dmac + "\n");
                     
        }
        else
        {

            msg = msgMatch.Value;
            src = srcMatch.Value;
            src = src.Trim();
            dst = dstMatch.Value;
            dst = dst.Trim();

            processedLine.Append(date + "|")
                        .Append(LSM + "|")
                        .Append(msg + "|")
                        .Append(src + "|")
                        .Append(dst + "\n");
                                 
        }

        return processedLine.ToString();

    }

    public bool TofinoModeChange(string logLine)
    {

        string modePattern = @"(Tofino System: [^|]+)";
        Match modeMatch = Regex.Match(logLine, modePattern);
        string mode = modeMatch.Value;

        if(mode.Contains("Change"))
            return true;
        
        else
            return false;

    }

    public string TofinoMode(string logLine)
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

            return "Error de ACL";

        }
        else if(Regex.IsMatch(logLine, ModbusPattern))
        {

            string patternCode = @"code (\d{2})";
            Match codeMatch = Regex.Match(logLine, patternCode);
            string code = codeMatch.Value;

            string patternError = @"\d{2}";
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

    private string modbusErrors(int code)
    {

        switch(code)
        {

            case 1:
                return "Read Coils";
            
            case 2: 
                return "Read Discrete Inputs";

            case 3:
                return "Read Holding Registers";

            case 4:
                return "Read Input Registers";

            case 5:
                return "Write Single Coil";

            case 6:
                return "Write Single Register";

            case 7:
                return "Read Exception Status";

            case 8:
                return "Diagnostics";

            case 11:
                return "Get Comm Event Counter";

            case 12: 
                return "Get Com Event Log";

            case 15:
                return "Write Multiple Coils";

            case 16:
                return "Write Multiple Registers";

            case 17:
                return "Report Server ID";

            case 20:
                return "Read File Record";

            case 21: 
                return "Write File Record";

            case 22:
                return "Mask Write Regiser";

            case 23:
                return "Read/Write Multiple Registers";

            case 24:
                return "Read FIFO Queue";

            case 43:
                return "Read Device Identification";

            default:
                return "Unknown Error";

        }

    }

}
