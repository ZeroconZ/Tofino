using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class RW : MonoBehaviour
{
    
    StreamReader reader;
    LogProcessor logProcessor = new LogProcessor();
    StringBuilder lineConc = new StringBuilder();

    bool firstRead = false;

    void Start()
    {

        string pathR = Application.dataPath + "/Docs" + "/Logs" + "/logstofino.txt"; 
        reader = new StreamReader(pathR);

        LogReader().ContinueWith(Task =>
        {

            if(Task.Exception != null)
            {

                Debug.LogError(Task.Exception);
            
            }

        });

    }

    private async Task LogReader()
    {

        int id = 0;

        while(!reader.EndOfStream)
        {
            
            string line;

            id++;
            line = await reader.ReadLineAsync();

            if(firstRead == false)
                modeTransfer(line);
            if(logProcessor.TofinoModeChange(line) == true)
                modeTransfer(line);

            line = logProcessor.lineProcessor(line);   
            line = id.ToString() + " " + line;

            EVTransfer(line);
            ErrorTransfer(line, id);

        }

    }

    void modeTransfer(string line)
    {

        string date = logProcessor.getDate(line);
        line = logProcessor.TofinoMode(line);

        if(firstRead == false)
        {

            MMO.instance.newMode(line, date);
            firstRead = true;

        }
        else
            MMO.instance.newMode(line, date);

    }

    void EVTransfer(string line)
    {

        EventVis.instance.newLog(line);

    }

    void ErrorTransfer(string line, int id)
    {
        
        string idS = id.ToString();
        string error = logProcessor.getError(line);

        EventNotif.instance.newNotif(error, idS);

    }

    void OnDestroy()
    {

        reader.Dispose();

    }

}

