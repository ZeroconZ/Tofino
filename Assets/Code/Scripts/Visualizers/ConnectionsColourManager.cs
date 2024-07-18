using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class CCM : MonoBehaviour
{

    public static CCM instance;
    EventProcessor logProcessor = new EventProcessor();
    
    public Material PC_TSA;
    public Material PLC_TSA;
    public Material TSA_VAR;
    public Material HMI_VAR;

    public GameObject PLC;
    public GameObject VAR;
    public GameObject PC;
    public GameObject HMI;

    void Awake()
    {

        if(CCM.instance == null)
            CCM.instance = this;

        else 
            DestroyImmediate(gameObject);

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
        PLC.SetActive(false);
        VAR.SetActive(false);            
        HMI.SetActive(false);
        PC.SetActive(false);       
        
       
        if(src.Trim() == "src=10.1.1.10" || SMAC.Trim() == "smac=00:80:f4:16:3b:4f") //Origen PLC
        {

            PLC_TSA.color = Color.red;

        }
        else if(src.Trim() == "src=10.1.1.12" || SMAC.Trim() == "smac=00:80:f4:dc:16:5f") //Origen Variador
        {

            TSA_VAR.color = Color.red;

        }
        else if(src.Trim() == "src=10.1.1.11" || SMAC.Trim() == "smac=00:80:f4:dc:16:5f") //Origen HMI
        {

            HMI_VAR.color = Color.red;

        }
        else if(src.Trim() == "src=10.1.1.101" || SMAC.Trim() == "smac=e0:d5:5e:df:e6:1a") //Origen desconocido
        {

            PC_TSA.color = Color.red;

        }


        if(dst.Trim() == "dst=10.1.1.10" || DMAC.Trim() == "dmac=00:80:f4:16:3b:4f") //Destino PLC
        {

            PLC_TSA.color = Color.red;
            PLC.SetActive(true);
            VAR.SetActive(false);            
            HMI.SetActive(false);
            PC.SetActive(false);
            

        }
        else if(dst.Trim() == "dst=10.1.1.12" || DMAC.Trim() == "smac=00:80:f4:dc:16:5f") //Destino Variador
        {

            TSA_VAR.color = Color.red;
            PLC.SetActive(false);
            VAR.SetActive(true);            
            HMI.SetActive(false);
            PC.SetActive(false);

        }
        else if(dst.Trim() == "dst=10.1.1.11" || DMAC.Trim() == "dmac=00:80:f4:dc:16:5f") //Destino HMI
        {

            HMI_VAR.color = Color.red;
            PLC.SetActive(false);
            VAR.SetActive(false);            
            HMI.SetActive(true);
            PC.SetActive(false);

        }
        else if(src.Trim() == "dst=10.1.1.101" || SMAC.Trim() == "dmac=e0:d5:5e:df:e6:1a") //Destino Desconocido
        {

            PC_TSA.color = Color.red;
            PLC.SetActive(false);
            VAR.SetActive(false);
            HMI.SetActive(false);
            PC.SetActive(true);

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

        PC_TSA.color = Color.green;
        PLC_TSA.color = Color.green;
        TSA_VAR.color = Color.green;
        HMI_VAR.color = Color.green;

    }

}
