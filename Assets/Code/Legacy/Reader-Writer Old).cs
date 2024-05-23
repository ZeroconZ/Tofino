/*using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RWO : MonoBehaviour
{
    
    Tofino tofino;
    StreamReader reader;
    StreamWriter writer;

    string rutaWLog(string type)
    {

        string pathW = "";

        switch (type)
        {

            case "F":

                pathW = Application.dataPath + "/Docs" + "/Logs" + "/logsFirewall.txt";
                break;

            case "E":

                pathW = Application.dataPath + "/Docs" + "/Logs" + "/logsEnforcer.txt";
                break;
            
            case "TS":

                pathW = Application.dataPath + "/Docs" + "/Logs" + "/logsSystem.txt";
                break;

            default:

                break;

        }
        
        return pathW;

    }

    void Start()
    {
        
        tofino = new Tofino();

        string pathR = Application.dataPath + "/Docs" + "/Logs" + "/logstofino.txt";
        reader = new StreamReader(pathR);

        writer = new StreamWriter(Application.dataPath + "/Docs" + "/Logs" + "/logsFirewall.txt", true);

    }

    string prevPath = "";

    void Update()
    {
        
        if(reader == null)
        {

            Debug.Log("Se han cometido errores");
            return;

        }

        if(!reader.EndOfStream)
        {

            string line = reader.ReadLine();
            string type = tofino.MsgType(line);
            
            string path = rutaWLog(type);
            
            if(path != prevPath)
            {

                writer.Close();
                writer = new StreamWriter(path, true);
                writer.WriteLine(line);

            }
            else
            {

                writer.WriteLine(line);

            }
            
            EventVis.instance.newLog(line);
            prevPath = path;

        }

    }

    void OnDestroy()
    {

        reader.Close();
        writer.Close();

    }

}*/
