using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;
using UnityEditor.MPE;




public class LogProcessor
{

    StringBuilder processedLine = new StringBuilder();

    public LogProcessor()
    {
    }

    public string lineProcesser(string logLine)
    {

        processedLine.Clear();

        string date = "";
        string LSM = "";
        string msg = "";
        string src = "";
        string dst = "";
        string smac = "";
        string dmac = "";

        string datePattern = @"^\w{3} \d{2} \d{2}:\d{2}:\d{2}";
        Match dateMatch = Regex.Match(logLine, datePattern);
        date = dateMatch.Value;

        string LSMPattern = @"(Tofino Firewall: [^|]+|Tofino ([^\s]+) Enforcer: [^|]+|Tofino Event Logger: [^|]+|Tofino System: [^|]+)";
        Match LSMMatch = Regex.Match(logLine, LSMPattern);
        LSM = LSMMatch.Value;

        string msgPattern = @"msg=((?!TofinoMode\b).)+";
        string srcPattern = @"src=([^ ]+)";
        string dstPattern = @"dst=([^ ]+)";

        Match msgMatch = Regex.Match(logLine, msgPattern);
        Match srcMatch = Regex.Match(logLine, srcPattern);
        Match dstMatch = Regex.Match(logLine, dstPattern);

        msg = msgMatch.Value;

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

        Debug.Log(logLine);
        return processedLine.ToString();

    }

    public bool TofinoModeChange(string logLine)
    {

        string modePattern = @"(Tofino System: [^|]+)";
        Match modeMatch = Regex.Match(logLine, modePattern);

        if(modeMatch.Success)
        {
            Debug.Log("Cambio \n");
            return true;
        }
        else
            return false;



    }

    public string TofinoMode(string logLine)
    {

        if(logLine.Contains("OPERATIONAL"))
        {

            return "OPERATIONAL";

        }
        else if(logLine.Contains("TEST"))
        {

            return "TEST";

        }
        else
        {

            return "ERROR";

        }

    }

}
