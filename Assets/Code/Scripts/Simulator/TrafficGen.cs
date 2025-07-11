using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using System.Text;
using Unity.VisualScripting;

public class TrafficGen : MonoBehaviour
{

    public static TrafficGen instance;

    int id;
    int src;
    int dst;
    int proto;
    bool TSAMode;
    bool ModbusAdv = false;

    StringBuilder Event = new StringBuilder();
    EventProcessor logProcessor = new EventProcessor();

    Dictionary<int, string> IPs = new Dictionary<int, string>();

    void Awake()
    {

        if(TrafficGen.instance == null)
            TrafficGen.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   


    void Start()
    {

        IPs.Add(0, "10.1.1.11");
        IPs.Add(1, "10.1.1.101");
        IPs.Add(2, "10.1.1.10");
        IPs.Add(3, "10.1.1.12");

        MMO.instance.newMode("TEST");
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

        if(!ModbusAdv)
            proto = val;

    }

    public void AdvModbus(int val)
    {

        if(ModbusAdv)
            proto = val + 4;

    }

    public void ShowAdvModbus(bool state)
    {

        ModbusAdv = state;
        proto = 1;

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
        Debug.Log(msg);

            if(src != dst)
            {

                int id = RE.instance.CheckRule(msg);

                CreateMsgDenied(id);
                MMO.instance.newMode(Event.ToString());

                if(id == -2)
                {

                    if(TSAMode)
                        CCM.instance.newEvent(Event.ToString());

                    else
                        return;

                }
                else
                {

                    if(TSAMode)
                        CCM.instance.newEvent(Event.ToString());

                    else
                    {

                        CCM.instance.allGreen();
                        CCM.instance.HideArrow();

                    }
                    
                    remitter(Event.ToString());

                }
                
            }
            else    
                return;

    }

    private void CreateMsgDenied(int id)
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
        if(proto < 1)
        {

            if(id == 0)
            {

                LSM = "Tofino Firewall: Logged Connection|";
                msg = "msg=Packet allowed and logged as specified in the ACL ";
                dpt_proto = "dpt=0 proto=IPv4/ICMP";

            }
            else if(id == 1)
            {

                LSM = "Tofino Firewall: ACL Violation|";
                msg = "msg=ACL violated due to incorrect network address(es), protocol, ports, rate limit and/or state ";
                dpt_proto = "dpt=0 proto=IPv4/ICMP";

            }

        }
        else if(proto > 1)
        {

            if(id == -2)
            {

                msg = "msg=Packet allowed and logged as specified in the ACL ";

            }
            else if(id == -1)
            {

                LSM = "Tofino Modbus/TCP Enforcer: Function Code List Check|";
                msg = $"msg=Function code {proto - 4} is not in permitted function code list ";
                dpt_proto = "dpt=502 proto=IPv4/TCP";

            }
            else if(id == 0)
            {

                LSM = "Tofino Firewall: Logged Connection|";
                msg = "msg=Packet allowed and logged as specified in the ACL ";
                dpt_proto = "dpt=502 proto=IPv4/TCP";

            }
            else if(id == 1)
            {

                LSM = "Tofino Firewall: ACL Violation|";
                msg = "msg=ACL violated due to incorrect network address(es), protocol, ports, rate limit and/or state ";
                dpt_proto = "dpt=502 proto=IPv4/TCP";

            }

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


