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

public class EventVis : MonoBehaviour//, IDragHandler
{

    //public Canvas canvas;
    //private RectTransform rectTransform;
    public static EventVis instance;
    public TextMeshProUGUI TextOnS;
    EventProcessor logProcessor = new EventProcessor();
    StringBuilder logLineConc = new StringBuilder();


    void Awake()
    {

        if(EventVis.instance == null)
            EventVis.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   
/*
    void Start()
    {

        rectTransform = GetComponent<RectTransform>();

    }


    void IDragHandler.OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }
*/
    public void newLog(string line, int id)
    {
        
        if(id % 200 == 0)
            removeOldLines(id);

        string proLine = logProcessor.eventProcessor(line);
        logLineConc.Append(id.ToString())
                   .Append(" ")
                   .AppendLine(proLine);

        logProcessor.getProtocol(line);

        updText();

    }

    private void updText()
    {

        TextOnS.text = logLineConc.ToString();

    }
    
    private void removeOldLines(int id)
    {

        int previousIndex = 0;
        int index = 0;

        for (int i = 0; i < 100; i++)
        {
            index = logLineConc.ToString().IndexOf('\n', previousIndex);
            if (index == -1)
            {

                break;
            }

            logLineConc.Remove(previousIndex, index - previousIndex + 1);
        }
    }

}
