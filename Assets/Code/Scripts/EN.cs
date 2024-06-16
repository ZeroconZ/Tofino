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

public class EventNotif : MonoBehaviour, IDragHandler
{

    public Canvas canvas;
    private RectTransform rectTransform;
    public static EventNotif instance;
    public TextMeshProUGUI ErrorNotif;
    public TextMeshProUGUI MoreInfo;

    private string error;
    private string id;

    private const float updInterv = 0.5f;
    private float lastUpd = 0f;

    void Awake()
    {

        if(EventNotif.instance == null)
            EventNotif.instance = this;

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

    public void newNotif(string line, string idS)
    {
        
        error = line;
        id = idS;

    }

    private void updText()
    {

        ErrorNotif.text = error;
        MoreInfo.text = "Read line " + id + " for more information";

    }



}
