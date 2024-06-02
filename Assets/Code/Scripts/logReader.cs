using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class RW : MonoBehaviour
{
    
    LogProcessor logProcessor = new LogProcessor();
    
    StreamReader reader;
    StringBuilder lineConc = new StringBuilder();

    [SerializeField] private int readLines = 1;

    public static class readerEnd
    {

        public static bool finished = false;

    }

    void Start()
    {

        string pathR = Application.dataPath + "/Docs" + "/Logs" + "/logstofino.txt"; 
        reader = new StreamReader(pathR);

        string line = reader.ReadLine();
        string mode = logProcessor.TofinoMode(line);

        MMO.instance.newMode(mode);

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
            string mode;

            for(int i = 0; i < readLines; i++)
            {

                line = await reader.ReadLineAsync();
                
                if(line == null)
                    break;
                
                lineConc.Append(logProcessor.lineProcesser(line));
                line = lineConc.ToString();

                if(logProcessor.TofinoModeChange(line) == true)
                {

                    mode = logProcessor.TofinoMode(line);
                    MMO.instance.newMode(mode);

                }

                



            }
            
            EventVis.instance.newLog(lineConc.ToString());
            Debug.Log(lineConc.ToString());
            lineConc.Clear();
            
        }

        if(reader.EndOfStream)
        {
            
            readerEnd.finished = true;

        }

    }


}

