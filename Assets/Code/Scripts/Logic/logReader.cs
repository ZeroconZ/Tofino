using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;
using System.Text.RegularExpressions;

public class logReader : MonoBehaviour
{

    LogProcessor logProcessor = new LogProcessor();


    string username = "dtuser";
    string password = "TofinoDT_2024";


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

            yield return new WaitForSeconds(0.05f);

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

        string line;
        int previousIndex = 0;
        int index;
        
        while ((index = logBuilder.ToString().IndexOf("<br>", previousIndex)) != -1)
        {

            line = logBuilder.ToString().Substring(previousIndex, index - previousIndex);
            Debug.Log(line);
            previousIndex = index + 4;

            if(Regex.IsMatch(line, "<end>"))
            {

                string lastLineDate = logProcessor.getDate(line);
                Debug.Log(lastLineDate);

            }

        }

    }

}
