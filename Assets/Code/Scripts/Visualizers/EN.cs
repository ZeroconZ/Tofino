using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;

public class EventNotif : MonoBehaviour
{

    public static EventNotif instance;
    public TextMeshProUGUI ErrorNotif;
    public TextMeshProUGUI MoreInfo;

    EventProcessor logProcessor = new EventProcessor();

    private string error;
    private string id;

    void Awake()
    {

        if(EventNotif.instance == null)
            EventNotif.instance = this;

        else 
            DestroyImmediate(gameObject);

    }       

    public void newNotif(string line, string idS)
    {
        
        error = logProcessor.getError(line);
        id = idS;

        updText();

    }

    private void updText()
    {

        ErrorNotif.text = error;
        MoreInfo.text = "Read line " + id + " for more information";

    }

}
