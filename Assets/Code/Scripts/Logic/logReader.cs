using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;
using System.Text.RegularExpressions;
using System;
using System.Globalization;

public class logReader : MonoBehaviour
{

    EventProcessor logProcessor = new EventProcessor();


    string username = "dtuser";
    string password = "TofinoDT_2024";

    string lastLineDate;
    string lastLineProcc;
    
    int id;
    bool firstReq = true;


    [ContextMenu("Read log")]
    void Start()
    {

        StartCoroutine(keepReading());

    }


    private IEnumerator keepReading()
    {

        while(true)
        {

            yield return StartCoroutine(APIReader());

            yield return new WaitForSeconds(0.5f);

        }

    }


    private IEnumerator APIReader()
    {

        UnityWebRequest webReader = UnityWebRequest.Get("https://aulaschneider.unileon.es/api/data/armario7/tofino-all");

        string auth = username + ":" + password;
        string authHeaderValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
        webReader.SetRequestHeader("Authorization", "Basic " + authHeaderValue);

        yield return webReader.SendWebRequest();

        if(webReader.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log("Error" + webReader.error);

        else
            divideLines(webReader.downloadHandler.text);

    }


    private void divideLines(string logData)
    {

        StringBuilder logBuilder = new StringBuilder(logData);

        string line = string.Empty;
        string previousLine = string.Empty;

        int previousIndex = 0;
        int index;
        
        while ((index = logBuilder.ToString().IndexOf("<br>", previousIndex)) != -1)
        {

            if(firstReq == true)
            {

                line = logBuilder.ToString().Substring(previousIndex, index - previousIndex);
                remitter(line, previousLine);
                previousIndex = index + 4;

            }
            else
            {

                line = logBuilder.ToString().Substring(previousIndex, index - previousIndex);
                previousIndex = index + 4;

                switch(dateCompare(lastLineDate, logProcessor.getDate(line)))
                {

                    case 0:
                        break;

                    case 1:
                        remitter(line, previousLine);
                        break;

                    case 2:
                        if(lastLineProcc == logProcessor.eventProcessor(line))
                            break;

                        else
                        {

                            remitter(line, previousLine);
                            break;

                        }
                        
                    default:
                        Debug.Log("Error");
                        break;

                }

                previousLine = line;

            }

        }

        firstReq = false;

        if (previousIndex < logBuilder.Length)
        {

            lastLineDate = logProcessor.getDate(previousLine);
            lastLineProcc = logProcessor.eventProcessor(previousLine);

        }

    }


    private int dateCompare(string oldDate, string newDate)
    {

        if(string.IsNullOrEmpty(oldDate) || string.IsNullOrEmpty(newDate))
            return 0;


        DateTime refDate = DateTime.ParseExact(oldDate, "MMM dd HH:mm:ss", CultureInfo.InvariantCulture);
        DateTime toCompDate = DateTime.ParseExact(newDate, "MMM dd HH:mm:ss", CultureInfo.InvariantCulture);

        if(refDate > toCompDate)
            return 0;

        else if(refDate < toCompDate)
            return 1;

        else
            return 2;
    
    }


    private void remitter(string line, string previousLine)
    {

        string line1 = logProcessor.eventProcessor(line);
        string previousLine1 = logProcessor.eventProcessor(previousLine);

        if(line.Equals(previousLine))
        {

            return;

        }
        else
        {

            id++;
            EventVis.instance.newLog(line, id);
            EventNotif.instance.newNotif(line, id.ToString());
            CCM.instance.newEvent(line);

            Debug.Log(line1);

        }

    }

}
