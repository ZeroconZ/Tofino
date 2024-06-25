using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Threading.Tasks;
using System.Text;
using UnityEngine.Networking;
using UnityEditor.PackageManager.Requests;

public class logReader : MonoBehaviour
{

    [ContextMenu("Read log")]

    async void readWeb()
    {

        Debug.Log("Generado solicitud");
        var result = await LogReader();
        Debug.Log(result);

    }

    string username = "dtuser";
    string password = "TofinoDT_2024";

    private async Task<string> LogReader()
    {

        UnityWebRequest webReader = UnityWebRequest.Get("https://aulaschneider.unileon.es/api/data/armario7/tofino-all");

        string auth = username + ":" + password;
        string authHeaderValue = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(auth));
        webReader.SetRequestHeader("Authorization", "Basic " + authHeaderValue);

        webReader.SendWebRequest();

        while(!webReader.isDone)
        {

            await Task.Yield();

        }

        if(webReader.result == UnityWebRequest.Result.ConnectionError)
        {
            
            Debug.Log("Error" + webReader.error);
            return webReader.error;

        }
        else
        {

            return webReader.downloadHandler.text;

        }

    }

}
