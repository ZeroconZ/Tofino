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

    [SerializeField] private int readLines = 1;

    bool firstRead = false;

    public static class readerEnd
    {

        public static bool finished = false;

    }

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

    public async Task LogReader()
    {

        int id = 0;

        while(!reader.EndOfStream)
        {
            
            string line;

            for(int i = 0; i < readLines; i++)
            {

                id++;
                line = await reader.ReadLineAsync();
                
                if(firstRead == false)
                {

                    MMO.instance.ModeOnStart(line);
                    Debug.Log(line);
                    firstRead = true;

                }

                if(line == null)
                    break;
                
                line = id.ToString() + " " + logProcessor.lineProcesser(line);
                lineConc.Append(line);
                line = lineConc.ToString();

                if(logProcessor.TofinoModeChange(line) == true)
                    MMO.instance.newMode(line);
                
            }
            
            EventVis.instance.newLog(lineConc.ToString());
            lineConc.Clear();
            
        }

        if(reader.EndOfStream)
        {
            
            readerEnd.finished = true;

        }

    }

    void OnDestroy()
    {

        reader.Dispose();

    }

}

