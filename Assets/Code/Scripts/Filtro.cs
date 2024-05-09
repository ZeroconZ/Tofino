using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Filtro : MonoBehaviour
{
    
    Tofino tofino;
    StreamReader reader;
    StreamWriter writer;

    string rutaWLog(string type)
    {

        string rutaW = "";

        switch (type)
        {

            case "F":

                rutaW = Application.dataPath + "/Docs" + "/Logs" + "/logsFirewall.txt";
                break;

            case "E":

                rutaW = Application.dataPath + "/Docs" + "/Logs" + "/logsEnforcer.txt";
                break;
            
            case "TS":

                rutaW = Application.dataPath + "/Docs" + "/Logs" + "/logsSystem.txt";
                break;

            default:

                break;

        }

        return rutaW;

    }

    void Start()
    {
        
        tofino = new Tofino();

        string rutaR = Application.dataPath + "/Docs" + "/Logs" + "/logstofino.txt";
        reader = new StreamReader(rutaR);

        writer = new StreamWriter(Application.dataPath + "/Docs" + "/Logs" + "/logsFirewall.txt");

    }

    string rutaAnt = "";

    void Update()
    {
        
        if(reader == null)
        {

            Debug.Log("No va");
            return;

        }

        if(!reader.EndOfStream)
        {

            string linea = reader.ReadLine();
            string type = tofino.MsgType(linea);
            
            string ruta = rutaWLog(type);

            writer.Close();
            writer = new StreamWriter(ruta, true);

            rutaAnt = ruta;

            writer.WriteLine(linea);
            Debug.Log(linea);
            Debug.Log(ruta);

        }

    }

    void OnDestroy()
    {

        reader.Close();

    }
}
