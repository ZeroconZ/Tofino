using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.IO;
using UnityEditor;

public class EventVis : MonoBehaviour
{

    public static EventVis instance;
    public TextMeshProUGUI TextOnS;

    void Awake()
    {

        if(EventVis.instance == null)
            EventVis.instance = this;

        else 
            DestroyImmediate(gameObject);

    }

    public void newLog(string line)
    {

        TextOnS.text = line + "\n" + "\n" + TextOnS.text;

    }

}
