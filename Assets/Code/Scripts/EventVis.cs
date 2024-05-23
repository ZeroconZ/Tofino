using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using TMPro;
using System.IO;
using UnityEditor;
using System.Text;

public class EventVis : MonoBehaviour
{

    [SerializeField] private int queue = 10;

    public static EventVis instance;
    public TextMeshProUGUI TextOnS;
    private Queue<string> logQueue = new Queue<string>();
    private StringBuilder logPrinter = new StringBuilder();


    void Awake()
    {

        if(EventVis.instance == null)
            EventVis.instance = this;

        else 
            DestroyImmediate(gameObject);

    }

    void Start()
    {

    }

    public void newLog(string line)
    {

        if(logQueue.Count > queue)
        {

            logQueue.Clear();

        }
        
        logQueue.Enqueue(line);

        TextOnS.text = string.Join("\n", logQueue.ToArray());

    }

}
