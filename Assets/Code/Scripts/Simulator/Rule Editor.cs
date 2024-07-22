using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class RE : MonoBehaviour
{

    public static RE instance;

    public TMP_Dropdown rulesDisplay;

    int id;
    int src;
    int dst;
    int proto;
    int block;
    int edit_create;

    Dictionary<int,(int srcR, int dstR, int protoR, int blockR)> Rules = new Dictionary<int,(int, int, int, int)>();
    Dictionary<int, string> Devices = new Dictionary<int, string>();

    StringBuilder ruleBuilder = new StringBuilder();

    void Awake()
    {

        if(RE.instance == null)
            RE.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    void Start()
    {

        Devices.Add(0, "HMI");
        Devices.Add(1, "PC");
        Devices.Add(2, "PLC");
        Devices.Add(3, "DRIVER");

        CCM.instance.hideArrow();

    }

    //Creating a rule
    public void SrcDev(int val)
    {

        src = val;

    }

    public void DstDev(int val)
    {

        dst = val;

    }

    public void ProtoRule(int val)
    {

        proto = val;

    }

    public void Block(int val)
    {

        block = val;

    }

    public void Create_Edit(int val)
    {

        edit_create = val;
        
    }

    public void Apply()
    {

        if(src == dst)
        {

            Debug.Log("Source and Destiny cannot be the same");

        }
        else if(Rules.ContainsValue((src, dst, proto, 1)) || Rules.ContainsValue((src, dst, proto, 0)))
        {
            
            Debug.Log("Repe");

        }
        else
        {

            if(edit_create == 0)
            {    

                id++;

                Rules.Add(id, (src, dst, proto, block));
                Debug.Log("Regla: " + id + " " + Rules[id]);

            }
            else
            {

                Rules[edit_create] = (src, dst, proto, block);
                Debug.Log("Se editó la regla " + id + " " + Rules[id]);

            }

        }

        AddToDropDown();

    }

    //Making the rule visible in the dropdown menu
    private void AddToDropDown()
    {

        string protoS;
        string blockS;

        if(proto == 0)
            protoS = "MODBUS";
        else    
            protoS = "ICMP";

        if(block == 1)
            blockS = "BLOCK";
        else
            blockS = "PASS";

        ruleBuilder.Append(id.ToString() + ":")
                   .Append(Devices[src] + ", ")
                   .Append(Devices[dst] + "|")
                   .Append(protoS + " ")
                   .Append(blockS);

        rulesDisplay.options.Add(new TMP_Dropdown.OptionData(ruleBuilder.ToString()));

        rulesDisplay.RefreshShownValue();

        ruleBuilder.Clear();

    }

    public bool CheckRule((int srcMsg, int dstMsg, int protoMsg) msg)
    {

        int key = -1;

        foreach (var keyI in Rules)
        {
            var value = keyI.Value;

            if (value.srcR == msg.srcMsg && value.dstR == msg.dstMsg && value.protoR == msg.protoMsg)
            {
                key = keyI.Key;
                break;
            }

        }

        if (key != -1 && Rules.TryGetValue(key, out var rule))
        {

            int isBlock = rule.blockR;
            
            if(isBlock == 1)
                return true;
            else    
                return false;

        }
        else
            return true;
 

    }

}
