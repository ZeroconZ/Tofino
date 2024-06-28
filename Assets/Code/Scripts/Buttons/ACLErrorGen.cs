using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ACLErrorGen : MonoBehaviour
{

    EventProcessor logProcessor = new EventProcessor();

    public void ButtonPressed()
    {

        int id = Random.Range(1,200);
        string idS = id.ToString();
        string ev = "Mar 12 13:36:14 169.254.2.2 F8: 1D:78:D0:17:C4 CEF:1|Tofino Security Standard|Tofino Xenon|03.2.03|200001|Tofino Firewall: ACL Violation|6|msg=ACL violated due to incorrect network address(es), protocol, ports, rate limit and/or state TofinoMode=TEST smac=00:01:23:4b:06:77 src=10.1.1.11 spt=10015 dmac=00:80:f4:16:3b:4f dst=10.1.1.10 dpt=502 proto=IPv4/TCP TofinoEthType=800 TofinoTTL=255 TofinoPhysIn=eth0";

        ev = logProcessor.getError(ev);

        EventNotif.instance.newNotif(ev,idS);

    }

}
