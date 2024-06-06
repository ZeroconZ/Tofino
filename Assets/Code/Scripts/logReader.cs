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

        while(!reader.EndOfStream)
        {
            
            string line;

            for(int i = 0; i < readLines; i++)
            {

                line = await reader.ReadLineAsync();
                
                if(line == null)
                    break;
                
                lineConc.Append(logProcessor.lineProcesser(line));
                line = lineConc.ToString();

                if(firstRead == false)
                {

                    MMO.instance.newMode(line);
                    firstRead = true;

                }

                if(logProcessor.TofinoModeChange(line) == true)
                    MMO.instance.ModeOnStart(line);
                
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

