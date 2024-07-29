using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Text;

public class TrafficGen : MonoBehaviour
{

    int id;
    int src;
    int dst;
    int proto;
    bool TSAMode;

    StringBuilder Event = new StringBuilder();
    EventProcessor logProcessor = new EventProcessor();

    Dictionary<int, string> IPs = new Dictionary<int, string>();

    void Start()
    {

        IPs.Add(0, "10.1.1.11");
        IPs.Add(1, "10.1.1.101");
        IPs.Add(2, "10.1.1.10");
        IPs.Add(3, "10.1.1.12");

        CCM.instance.HideArrow();

    }
    
    public void SrcMsg(int val)
    {

        src = val;

    }
    
    public void DstMsg(int val)
    {

        dst = val;

    }

    public void ProtoMsg(int val)
    {

        proto = val;

    }

    public void TSAMODE(int val)
    {

        if(val == 0)
            TSAMode = false;
        else    
            TSAMode = true;

    }

    public void CheckWhitelist()
    {

        var msg = (src, dst, proto);
        print("TE LLAMO");
        if(src != dst)
        {

            if(RE.instance.CheckRule(msg))
            {

                CreateMsg();

                if(TSAMode)
                    CCM.instance.newEvent(Event.ToString());
                else
                    CCM.instance.allGreen();

                remitter(Event.ToString());

            }
                
        }
        else    
            return;

    }

    private void CreateMsg()
    {

        string LSM = string.Empty;
        string srcIP;
        string dstIP;
        string msg = string.Empty;
        string TSAModeS;
        string dpt_proto = string.Empty;

        IPs.TryGetValue(src, out string srcS);
        IPs.TryGetValue(dst, out string dstS);  

        //Getting the date
        DateTime now = DateTime.Now;
        now = now.ToUniversalTime();
        string logDate = now.ToString("MMM dd HH:mm:ss", CultureInfo.CreateSpecificCulture("en-US"));

        //Assigning the info
        if(proto == 0)
        {

            LSM = "Tofino Firewall: ACL Violation|";
            msg = "msg=ACL violated due to incorrect network address(es), protocol, ports, rate limit and/or state ";
            dpt_proto = "dpt=502 proto=IPv4/TCP";
            
        }
        else if(proto == 1)
        {

            LSM = "Tofino Firewall: ACL Violation|";
            msg = "msg=ACL violated due to incorrect network address(es), protocol, ports, rate limit and/or state ";
            dpt_proto = "dpt=0 proto=IPv4/ICMP";

        }
        
        if(!TSAMode)
            TSAModeS = "TofinoMode=TEST ";
        else
            TSAModeS = "TofinoMode=OPERATIONAL ";
            

        srcIP = "src=" + srcS;
        dstIP = "dst=" + dstS; 
        
        
        //Creating the event
        Event.Append(logDate + " ")
             .Append(LSM)
             .Append(msg)
             .Append(TSAModeS)
             .Append(srcIP + " ")
             .Append(dstIP + " ")
             .Append(dpt_proto);
        
    }

    private void remitter(string line)
    {

            id++;
            EventVis.instance.newLog(line, id);
            EventNotif.instance.newNotif(line, id.ToString());
            MMO.instance.newMode(line);

            Event.Clear();

    }

}


