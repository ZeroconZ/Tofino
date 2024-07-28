using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Text;
using TMPro;
using UnityEngine;

public class MoveRulesDD : MonoBehaviour
{
    
    public TMP_Dropdown rules;

    int index;

    public void MoveUp()
    {
        
        string ruleMovedUp = GetText(index);
        string ruleMovedDown = GetText(index-1);

    }

    public void RuleSelected(int val)
    {

        index = val;

    }

    public string GetText(int val)
    {

        string rule = rules.options[val].text;
        return rule;

    }

}
