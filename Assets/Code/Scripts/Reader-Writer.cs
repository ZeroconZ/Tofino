using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;

public class RW : MonoBehaviour
{
    
    StreamReader reader;
    StringBuilder lineConc = new StringBuilder();

    [SerializeField] private int readLines = 4;

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

            for(int i = 0; i <= readLines; i++)
            {

                line = await reader.ReadLineAsync();
                
                if(line == null)
                    break;
                

                lineConc.Append(line + "\n");

            }

            EventVis.instance.newLog(lineConc.ToString());
            Debug.Log(lineConc);
            lineConc.Clear();

        }

    }


}

