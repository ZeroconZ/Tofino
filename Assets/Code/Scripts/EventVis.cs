using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;

public class EventVis : MonoBehaviour
{


    public static EventVis instance;
    public TextMeshProUGUI TextOnS;
    StringBuilder lineConc = new StringBuilder();


    void Awake()
    {

        if(EventVis.instance == null)
            EventVis.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    public void newLog(string line)
    {

        TextOnS.text = TextOnS.text + line;

    }

}
