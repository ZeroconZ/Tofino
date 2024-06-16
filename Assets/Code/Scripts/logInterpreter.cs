using System.Collections;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using UnityEngine;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using System;
using UnityEditor.PackageManager;

public class LogInterpreter
{

    StringBuilder logShow = new StringBuilder();

    public LogInterpreter()
    {
    }

    public string msgError(string logLine)
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

            return "Error desconocido";

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
                return "Error desconocido";

        }

    }



}
