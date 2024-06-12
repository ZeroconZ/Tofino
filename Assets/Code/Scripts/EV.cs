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

public class EventVis : MonoBehaviour, IDragHandler
{

    public Canvas canvas;
    private RectTransform rectTransform;
    public static EventVis instance;
    public TextMeshProUGUI TextOnS;
    StringBuilder logLineConc = new StringBuilder();

    private const float updInterv = 0.5f;
    private float lastUpd = 0f;
    private string logLine;

    void Awake()
    {

        if(EventVis.instance == null)
            EventVis.instance = this;

        else 
            DestroyImmediate(gameObject);

    }   

    void Start()
    {

        rectTransform = GetComponent<RectTransform>();

    }

    void Update()
    {

        lastUpd += Time.deltaTime;

        if(lastUpd >= updInterv)
        {
            
            updText();
            lastUpd = 0f;

        }

    }

    void IDragHandler.OnDrag(PointerEventData eventData)
    {

        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

    }

    public void newLog(string line)
    {
        
        logLineConc.AppendLine(line);

    }

    private void updText()
    {

        TextOnS.text = logLineConc.ToString();

    }



}
