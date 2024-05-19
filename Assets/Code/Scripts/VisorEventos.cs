using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.IO;
using UnityEditor;

public class VisorEventos : MonoBehaviour
{

    public TextMeshProUGUI TextOnS;

    Tofino tofino;
    StreamReader reader;

    public void newLine(string newLine)
    {
        TextOnS.text += newLine;
        Canvas.ForceUpdateCanvases();
        GetComponentInParent<ScrollRect>().verticalNormalizedPosition = 0f;
    }

    void Start()
    {
         
        TextOnS.text = "";

        tofino = new Tofino();

        string rutaR = Application.dataPath + "/Docs" + "/Logs" + "/logstofino.txt";
        reader = new StreamReader(rutaR);

    }

    void Update()
    {
        
        if(reader == null)
        {

            Debug.Log("Se han cometido errores");
            return;

        }

        if(!reader.EndOfStream)
        {

            string linea = reader.ReadLine();
            string type = tofino.MsgType(linea);

            newLine(type);

        }

    }

    void OnDestroy()
    {

        reader.Close();

    }
}
