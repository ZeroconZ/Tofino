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

    List<string> events = new List<string>();


    string username = "dtuser";
    string password = "TofinoDT_2024";

    string lastLineDate;
    string oldLine = string.Empty;
    
    int id;
    bool firstReq = true;
    string last;
    string lastButOne;



    [ContextMenu("Read log")]
    void Start()
    {

        StartCoroutine(keepReading());

    }


    private IEnumerator keepReading()
    {

        while(true)
        {

            yield return StartCoroutine(APIRequest());

            yield return new WaitForSeconds(0.250f);

        }

    }


    private IEnumerator APIRequest()
    {

        UnityWebRequest webReader = UnityWebRequest.Get("https://aulaschneider.unileon.es/api/data/armario7/tofino-all");

        string auth = username + ":" + password;
        string authHeaderValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
        webReader.SetRequestHeader("Authorization", "Basic " + authHeaderValue);

        yield return webReader.SendWebRequest();

        if(webReader.result == UnityWebRequest.Result.ConnectionError)
            Debug.Log("Error" + webReader.error);

        else
            NewEvents(webReader.downloadHandler.text);

    }

    private void NewEvents(string log)
    {

        StringBuilder logBuilder = new StringBuilder(log);

        int previousIndex = 0;
        int index;
        
        while ((index = logBuilder.ToString().IndexOf("<br>", previousIndex)) != -1)
        {

            string line = logBuilder.ToString().Substring(previousIndex, index - previousIndex);
            previousIndex = index + 4;

            events.Add(line);

        }

        if(firstReq)
        {

            last = events[events.Count - 1];
            lastButOne = events[events.Count - 2];
            MMO.instance.newMode(events[0]);
            firstReq = false;

        }
        else
        {

            int lastIndexPreviousReq = FindEvents(last, lastButOne);

            if(lastIndexPreviousReq == events.Count)
                return;
            
            for(int i = lastIndexPreviousReq; i < events.Count - 1; i++)
            {

                Remitter(events[i]);

            }

            last = events[events.Count - 1];
            lastButOne = events[events.Count - 2];

        }

        events.Clear();

    }

    private int FindEvents(string lastEvent, string lastButOneEvent)
    {

        for (int i = 0; i < events.Count - 1; i++)
        {

            if(events[i].Equals(lastEvent) && events[i-1].Equals(lastButOneEvent))
                return i;

        }

        return 9999;

    }

    private void Remitter(string line)
    {

            id++;
            EventVis.instance.newLog(line, id);
            EventNotif.instance.newNotif(line, id.ToString());
            CCM.instance.newEvent(line);

            if(logProcessor.getModeChange(line) == true)
                MMO.instance.newMode(line);

            Debug.Log(line);

    }

}
