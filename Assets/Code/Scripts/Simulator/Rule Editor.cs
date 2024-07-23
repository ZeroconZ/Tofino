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


    //Rules values
    int id;
    int src;
    int dst;
    int proto;
    int block;
    int ruleDisplayIndex;

    //Dictionary where rules are stored
    Dictionary<int,(int srcR, int dstR, int protoR, int blockR)> Rules = new Dictionary<int,(int, int, int, int)>();
    //Dictionary that relates devices variables to their names
    Dictionary<int, string> Devices = new Dictionary<int, string>();

    StringBuilder ruleBuilder = new StringBuilder();

    //UI elements to move rules up or down
    bool Arrows = false;
    public UnityEngine.UI.Button ArrowUp;
    public UnityEngine.UI.Button ArrowDown;
    public Sprite ArrowUpActive, ArrowUpDeactiv, ArrowDownActive, ArrowDownDeactiv;

    //MovingRules elements
    public TMP_Dropdown rules;

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

    //Values for a rule
    public void SrcDev(int val)
    {

        src = val;

        return;

    }

    public void DstDev(int val)
    {

        dst = val;

        return;

    }

    public void ProtoRule(int val)
    {

        proto = val;

        return;

    }

    public void Block(int val)
    {

        block = val;

        return;

    }

    public void Create_Edit(int val)
    {

        ruleDisplayIndex = val;

        return;
        
    }

    public void CreateRule()
    {

        if(src == dst)
        {

            Debug.Log("Source and Destiny cannot be the same");

        }
        else
        {

            if(ruleDisplayIndex == 0)
            {    

                if(Rules.ContainsValue((src, dst, proto, 1)) || Rules.ContainsValue((src, dst, proto, 0)))
                {
                    
                    Debug.Log("Repe");

                }
                else
                {

                    id++;

                    Rules.Add(id, (src, dst, proto, block));
                    Debug.Log("Rule: " + id + " " + Rules[id]);
                    AddToDropDown(TextDisplayRule(id, (src, dst, proto, block)));
                
                }

            }
            else
            {

                Rules[ruleDisplayIndex] = (src, dst, proto, block);
                Debug.Log("Edited rule " + ruleDisplayIndex + " " + Rules[ruleDisplayIndex]);
                ReplaceRule(ruleDisplayIndex, TextDisplayRule(ruleDisplayIndex, (src, dst, proto, block)));

            }

        }

        return;

    }

    //Creating the text for the rule
    private string TextDisplayRule(int index, (int srcAdd, int dstAdd, int protoAdd, int blockAdd)rule)
    {

        string ruleTxt;

        string protoS;
        string blockS;

        if(rule.Item3 == 0)
            protoS = "MODBUS";
        else    
            protoS = "ICMP";

        if(rule.Item4 == 1)
            blockS = "BLOCK";
        else
            blockS = "PASS";

        ruleBuilder.Append(index.ToString() + ":")
                   .Append(Devices[rule.Item1] + ", ")
                   .Append(Devices[rule.Item2] + "|")
                   .Append(protoS + " ")
                   .Append(blockS);
        
        ruleTxt = ruleBuilder.ToString();
        ruleBuilder.Clear();

        return ruleTxt;

    }

    //Making the rule visible in the dropdown menu
    private void AddToDropDown(string newRule)
    {

        rulesDisplay.options.Add(new TMP_Dropdown.OptionData(newRule));

        rulesDisplay.RefreshShownValue();

        return;

    }

    //Replacing a existing rule
    public void ReplaceRule(int index, string newRule)
    {

        rulesDisplay.options[index] = new TMP_Dropdown.OptionData(newRule);

        rulesDisplay.RefreshShownValue();

        return;

    }


    //Check if the rule exists
    public bool CheckRule((int srcMsg, int dstMsg, int protoMsg) msg)
    {

        int key = -1;
        print("ME LLAMAN");
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

    //Toggle arrows to move up or down the rules in the hierarchy
    public void ToggleArrows(int val)
    {

        if(val == 0)
        {

            ArrowUp.image.sprite = ArrowUpDeactiv;
            ArrowDown.image.sprite = ArrowDownDeactiv;

        }
        else 
        {

            ArrowUp.image.sprite = ArrowUpActive;
            ArrowDown.image.sprite = ArrowDownActive;

        }

        return;

    }


    //Moving the rules in the hierarchy

    public void MoveUp()
    {
        
        var valueToMove = Rules[ruleDisplayIndex];
        var ValueDisplaced = Rules[ruleDisplayIndex - 1];

        string ruleUp = TextDisplayRule(ruleDisplayIndex-1, valueToMove);
        print(ruleUp);

        string ruleDown = TextDisplayRule(ruleDisplayIndex, ValueDisplaced);
        print(ruleDown);

        Rules[ruleDisplayIndex - 1] = valueToMove;
        Rules[ruleDisplayIndex] = ValueDisplaced;
        ReplaceRule(ruleDisplayIndex -1 ,ruleUp);
        ReplaceRule(ruleDisplayIndex, ruleDown);

        return;

    }

    public void MoveDown()
    {

        var valueToMove = Rules[ruleDisplayIndex];
        var ValueDisplaced = Rules[ruleDisplayIndex + 1];

        string ruleUp = TextDisplayRule(ruleDisplayIndex+1, valueToMove);
        print(ruleUp);

        string ruleDown = TextDisplayRule(ruleDisplayIndex, ValueDisplaced);
        print(ruleDown);

        Rules[ruleDisplayIndex + 1] = valueToMove;
        Rules[ruleDisplayIndex] = ValueDisplaced;
        ReplaceRule(ruleDisplayIndex +1 ,ruleUp);
        ReplaceRule(ruleDisplayIndex, ruleDown);

        return;

    }
}
