using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModbusErrorGen : MonoBehaviour
{

    EventProcessor logProcessor = new EventProcessor();

    [SerializeField] string functionID;

    public void ButtonPressed()
    {

        int id = Random.Range(1,200);
        string idS = id.ToString();
        Debug.Log(functionID);

        string ev = "Mar 12 13:37:21 169.254.2.2 F8: 1D:78:D0:17:C4 CEF:1|Tofino Security Standard|Tofino Xenon|03.2.03|300008|Tofino Modbus/TCP Enforcer: Function Code List Check|6|msg=Function code " + functionID +" is not in permitted function code list TofinoMode=OPERATIONAL smac=e0:d5:5e:df:e6:1a src=10.1.1.101 spt=12801 dmac=00:80:f4:16:3b:4f dst=10.1.1.10 dpt=502 proto=IPv4/TCP TofinoEthType=800 TofinoTTL=128 TofinoPhysIn=eth0";
        Debug.Log(ev);
        ev = logProcessor.getError(ev);

        EventNotif.instance.newNotif(ev,idS);

    }

}
