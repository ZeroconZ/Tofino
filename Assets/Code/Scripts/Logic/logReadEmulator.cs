using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class logEmu : MonoBehaviour
{
    
    StreamReader reader;
    EventProcessor logProcessor = new EventProcessor();
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
                MMO.instance.newMode(line);
            if(logProcessor.getModeChange(line) == true)
                MMO.instance.newMode(line);
 
            string idS = id.ToString();

            EventVis.instance.newLog(line, idS);
            EventNotif.instance.newNotif(line, idS);

        }

    }

    void OnDestroy()
    {

        reader.Dispose();

    }

}

