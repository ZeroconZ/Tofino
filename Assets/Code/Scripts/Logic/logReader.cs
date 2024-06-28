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

            yield return new WaitForSeconds(1f);

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
        logBuilder.Append("<end>");

        string line = string.Empty;
        string previousLine = string.Empty;

        int previousIndex = 0;
        int index;
        
        while ((index = logBuilder.ToString().IndexOf("<br>", previousIndex)) != -1)
        {

            if(firstReq)
            {

                previousLine = line;
                line = logBuilder.ToString().Substring(previousIndex, index - previousIndex);
                Debug.Log(line);
                previousIndex = index + 4;
                firstReq = false;

            }
            else
            {

                previousLine = line;
                line = logBuilder.ToString().Substring(previousIndex, index - previousIndex);
                previousIndex = index + 4;

                switch(dateCompare(lastLineDate, logProcessor.getDate(line)))
                {

                    case 0:
                        Debug.Log("Era viejito");
                        break;

                    case 1:
                        Debug.Log("Era nuevito");
                        break;

                    case 2:
                        Debug.Log("Son igualitos");
                        break;

                    default:
                        Debug.Log("Error");
                        break;

                }

            }

        }

        if (previousIndex < logBuilder.Length)
        {

            lastLineDate = logProcessor.getDate(previousLine);
            lastLineProcc = logProcessor.eventProcessor(previousLine);
            Debug.Log(lastLineDate);

        }

    }

    private int dateCompare(string oldDate, string newDate)
    {

        DateTime refDate = DateTime.ParseExact(oldDate, "MMM dd HH:mm:ss", CultureInfo.InvariantCulture);
        DateTime toCompDate = DateTime.ParseExact(newDate, "MMM dd HH:mm:ss", CultureInfo.InvariantCulture);

        if(refDate > toCompDate)
        {

            Debug.Log("El evento era anterior");
            return 0;

        }
        else if(refDate < toCompDate)
        {

            Debug.Log("El evento era posterior");
            return 1;

        }
        else
        {

            Debug.Log("Son coetaneos");
            return 2;

        }

    }

}
