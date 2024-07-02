using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CCM : MonoBehaviour
{

    public static CCM instance;
    EventProcessor logProcessor = new EventProcessor();
    
    public Material OD_TSA;
    public Material PLC_TSA;
    public Material TSA_VAR;

    void Awake()
    {

        if(CCM.instance == null)
            CCM.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    void Start()
    {
        
        OD_TSA.color = Color.red;
        PLC_TSA.color = Color.red;
        TSA_VAR.color = Color.red;

    }

    void OnDestroy()
    {

        allGreen();

    }

    public void newEvent(string logLine)
    {

        string msg = logProcessor.getMsg(logLine);
        string src = logProcessor.getSrcIP(logLine);
        string dst = logProcessor.getDstIP(logLine);
        string SMAC = logProcessor.getSMAC(logLine);
        string DMAC = logProcessor.getDMAC(logLine);


        allGreen();
       
        if(src == "src=10.1.1.10" || SMAC == "smac=00:80:f4:16:3b:4f") //Origen PLC
        {

            PLC_TSA.color = Color.red;

        }
        else if(src == "src=10.1.1.12" || SMAC == "smac=00:80:f4:dc:16:5f") //Origen Variador
        {

            TSA_VAR.color = Color.red;

        }
        else //Origen desconocido
        {

            OD_TSA.color = Color.red;

        }


        if(dst == "dst=10.1.1.10" || DMAC == "dmac=00:80:f4:16:3b:4f") //Destino PLC
        {

            PLC_TSA.color = Color.red;
            

        }
        else if(dst == "dst=10.1.1.12" || DMAC == "smac=00:80:f4:dc:16:5f") //Destino Variador
        {

            TSA_VAR.color = Color.red;

        }
        else //Destino Desconocido
        {

            OD_TSA.color = Color.red;

        }
        

    }

    private int Error(string msg)
    {

        string ACLPatt = @"ACL";
        string ModbusPatt = @"Modbus/TCP";
        
        if(Regex.IsMatch(msg, ACLPatt)) //ACL
        {
            
            return 0;

        }
        else if(Regex.IsMatch(msg, ModbusPatt)) //Modbus 
        {

            return 1;

        }
        else
        {

            return 2;

        }

    }

    private void allGreen()
    {

        OD_TSA.color = Color.green;
        PLC_TSA.color = Color.green;
        TSA_VAR.color = Color.green;

    }

}
