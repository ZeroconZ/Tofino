using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Xml.Linq;


public class Tofino
{

    public Tofino()
    {
    }

    public string MsgType(string linea)
    {

        if(linea.Contains("Enforcer"))
        {
            
            return "E";

        }
        else if(linea.Contains("Firewall"))
        {

            return "F";

        }
        else if(linea.Contains("Tofino System") ||linea.Contains("Logger"))
        {

            return "TF";

        }
        else
        {

            return "X";

        }
        
    }

    public bool TofinoMode(string linea)
    {

        if(linea.Contains("OPERATIONAL"))
        {

            return true;

        }
        else if(linea.Contains("TEST"))
        {

            return false;

        }
        else
        {

            return false;

        }

    }

}
